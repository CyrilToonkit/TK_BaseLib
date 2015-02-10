using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TK.BaseLib.Geometry
{
    public class CG_PointData
    {
        const double EPSILON = 0.000000000000001;

        public CG_PointData()
        {

        }

        public CG_PointData(Array inValues, List<string> inFields, string inType)
        {
            mValues = inValues;
            mFields = inFields;

            mLengths = new int[inValues.Rank];
            for (int i = 0; i < mLengths.Length; i++)
            {
                mLengths[i] = mValues.GetLength(i);
            }

            mType = inType;
        }

        string mType;
        [XmlElementAttribute(Order = 1)]
        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        int mCompression = 2;
        [XmlElementAttribute(Order = 2)]
        public int Compression
        {
            get { return mCompression; }
            set { mCompression = value; }
        }

        int[] mLengths = new int[3];
        [XmlElementAttribute(Order = 3)]
        public int[] Lengths
        {
            get { return mLengths; }
            set { mLengths = value; }
        }

        [XmlIgnore]
        public int PointCount
        {
            get { return mLengths[Rank - 1]; }
        }

        [XmlIgnore]
        public int Rank
        {
            get { return mLengths.Length; }
        }

        List<string> mFields = new List<string>();
        [XmlElementAttribute(Order = 4)]
        public List<string> Fields
        {
            get { return mFields; }
            set { mFields = value; }
        }

        Array mValues;
        [XmlIgnore]
        public Array InternalValues
        {
            get
            {
                if (mCompression == 2 && (mValues == null || mValues.Length == 0))
                {
                    mValues = Array.CreateInstance(typeof(object), Lengths);

                    int counter = 0;
                    int Dim0, Dim1;

                    for (int i = 0; i < mValuesMap.Count; i++)
                    {
                        for (int j = 0; j < mValuesMap[i]; j++)
                        {
                            Dim1 = (counter % PointCount);

                            if (Rank == 1)
                            {
                                mValues.SetValue(mCompressedValues[i], Dim1);
                            }
                            else
                            {
                                Dim0 = (counter / PointCount);
                                mValues.SetValue(mCompressedValues[i], Dim0, Dim1);
                            }

                            counter++;
                        }
                    }
                }

                return mValues;
            }
            set { mValues = value; }
        }

        List<int> mValuesMap;
        [XmlElementAttribute(Order = 5)]
        public List<int> Map
        {
            get
            {
                if (mCompression > 1 && (mValuesMap == null || mValuesMap.Count == 0))
                {
                    List<int> map = new List<int>();
                    int mapEnum = -1;
                    double curVal;
                    double oldVal = double.NaN;
                    mCompressedValues = new List<double>();

                    for (int i = 0; i < PointCount * mLengths[0]; i++)
                    {
                        int Dim0 = (i / PointCount);
                        int Dim1 = (i % PointCount);

                        if (mCompression > 2)
                        {
                            curVal = Math.Round((double)InternalValues.GetValue(Dim0, Dim1), Math.Max(0, 15 - mCompression));
                        }
                        else
                        {
                            curVal = (double)InternalValues.GetValue(Dim0, Dim1);
                        }

                        if (oldVal == curVal || (curVal > EPSILON && TypesHelper.DoubleIsFuzzyEqual(oldVal, curVal, Math.Pow(Math.Max(0, 15 - mCompression), -1))))
                        {
                            map[mapEnum]++;
                        }
                        else
                        {
                            map.Add(1);
                            mapEnum++;
                            mCompressedValues.Add(curVal);
                            oldVal = curVal;
                        }
                    }

                    mValuesMap = map;
                }

                return mValuesMap;
            }

            set
            {
                mValuesMap = value;
            }
        }

        List<double> mCompressedValues;
        [XmlElementAttribute(Order = 6)]
        public List<double> Values
        {
            get
            {
                return mCompressedValues;
            }

            set
            {
                mCompressedValues = value;
            }
        }

        [XmlElementAttribute(Order = 7)]
        public double[] XmlValues
        {
            get
            {
                if(mCompression == 1)
                {
                    double[] a = new double[PointCount * mLengths[0]];
                    for (int i = 0; i < a.Length; i++)
                    {
                        int Dim0 = (i / PointCount);
                        int Dim1 = (i % PointCount);

                        a[i] = (double)InternalValues.GetValue(Dim0, Dim1);
                    }

                    return a;
                }

                return new double[0];
            }

            set
            {
                if(mCompression == 1)
                {
                    InternalValues = Array.CreateInstance(typeof(object), mLengths);
                    for (int i = 0; i < value.Length; i++)
                    {
                        int Dim0 = (i / PointCount);
                        int Dim1 = (i % PointCount);

                        InternalValues.SetValue(value[i], Dim0, Dim1);
                    }
                }

                ConsolidateFields();
            }
        }

        private void ConsolidateFields()
        {
            Dictionary<string, List<double>> fieldsValues = new Dictionary<string, List<double>>();

            int fieldIndex = 0;
            bool needsUpdate = false;

            //Force an update of InternalValues
            Array mockValues = InternalValues;

            foreach (string field in mFields)
            {
                List<double> fieldValues = new List<double>();
                for (int i = 0; i < PointCount; i++)
                {
                    fieldValues.Add((double)mValues.GetValue(fieldIndex, i));
                }

                if (fieldsValues.ContainsKey(field))
                {
                    needsUpdate = true;

                    //Add field values to stored duplicate
                    List<double> oldValues = fieldsValues[field];
                    int valIndex = 0;
                    foreach (double dupValue in fieldValues)
                    {
                        oldValues[valIndex] += dupValue;
                        valIndex++;
                    }

                    fieldsValues[field] = oldValues;
                }
                else
                {

                    fieldsValues.Add(field, fieldValues);
                }

                fieldIndex++;
            }

            if(needsUpdate)
            {
                mFields = new List<string>(fieldsValues.Keys);
                mLengths[0] = mFields.Count;
                mValues = Array.CreateInstance(typeof(object), Lengths);

                for (int i = 0; i < mFields.Count; i++ )
                {
                    for (int j = 0; j < fieldsValues[mFields[i]].Count; j++)
                    {
                        mValues.SetValue(fieldsValues[mFields[i]][j], i, j);
                    }
                }
            }
        }

        public Array GetFieldWeigths(string inField)
        {
            int index = Fields.IndexOf(inField);

            Array FieldValues = Array.CreateInstance(typeof(object), PointCount);

            Array inValues = InternalValues;

            if (index != -1)
            {
                for (int i = 0; i < PointCount;i++ )
                {
                    FieldValues.SetValue(inValues.GetValue(index, i), i);
                }
            }

            return FieldValues;
        }

        public List<DeformerPointWeight> GetPointWeigths(int inPointIndex)
        {
            List<DeformerPointWeight> PWs = new List<DeformerPointWeight>();

            if (inPointIndex > -1 && inPointIndex < PointCount)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    double weight = (double)mValues.GetValue(i, inPointIndex);
                    if (weight > EPSILON)
                    {
                        PWs.Add(new DeformerPointWeight(i, inPointIndex, weight));
                    }
                }
            }

            return PWs;
        }

        //Compatibility Methods

        public bool isPointsCompatible(CG_PointData inData)
        {
            return inData.PointCount == PointCount;
        }

        public bool isFieldsCompatible(CG_PointData inData)
        {
            if (Fields.Count == inData.Fields.Count)
            {
                foreach (string field in inData.Fields)
                {
                    if (!Fields.Contains(field))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public bool isCompatible(CG_PointData inData)
        {
            return isPointsCompatible(inData) && isFieldsCompatible(inData);
        }

        //Transposition Methods

        public CG_PointData TransIndices(List<int> inIndices)
        {
            return new CG_PointData(InternalValues, Fields, Type);
        }

        public CG_PointData LevelValues(double inValue, double Tolerance)
        {
            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), Fields.Count, PointCount), new List<string>(), Type);
            bool changed = false;

            for (int i = 0; i < Fields.Count; i++)
            {
                modified.Fields.Add(Fields[i]);

                for (int j = 0; j < PointCount; j++)
                {
                    if (TypesHelper.DoubleIsFuzzyEqual((double)InternalValues.GetValue(i, j), inValue, Tolerance))
                    {
                        modified.InternalValues.SetValue(0.0, i, j);
                        changed = true;
                    }
                    else
                    {
                        modified.InternalValues.SetValue((double)InternalValues.GetValue(i, j), i, j);
                    }
                }
            }

            return changed ? modified : this;
        }

        public CG_PointData RemoveFields(List<string> notFoundFields)
        {
            int newCount = Fields.Count - notFoundFields.Count;

            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), newCount, PointCount), new List<string>(), Type);

            int counter = 0;
            string field;

            bool[] contains = new bool[Fields.Count];
            for(int h = 0 ; h < Fields.Count; h++)
            {
                field = Fields[h];
                contains[h] = notFoundFields.Contains(field);
                if (!contains[h])
                {
                    modified.Fields.Add(field);
                }
            }

            for (int i = 0; i < PointCount; i++)
            {
                counter = 0;
                for(int j = 0; j < Fields.Count; j++)
                {
                    if (!contains[j])
                    {
                        modified.InternalValues.SetValue(InternalValues.GetValue(j, i), counter, i);
                        counter++;
                    }
                }
            }

            return modified;
        }

        public CG_PointData RemoveFieldsWithValue(double inValue, double Tolerance)
        {
            List<string> MatchString = new List<string>();
            bool match;

            for (int i = 0; i < Fields.Count; i++)
            {
                match = true;
                for (int j = 0; j < PointCount; j++)
                {
                    if (!TypesHelper.DoubleIsFuzzyEqual(inValue, (double)InternalValues.GetValue(i, j), Tolerance))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    MatchString.Add(Fields[i]);
                }
            }

            return RemoveFields(MatchString);
        }

        public int LimitFields(int inNumFields)
        {
            int removed = 0;
            for (int i = 0; i < PointCount; i++)
            {
                List<DeformerPointWeight> PW = GetPointWeigths(i);
                PW.Sort();
                PW.Reverse();
                int delta = PW.Count - inNumFields;
                if (delta > 0)
                {
                    for (int d = delta; d > 0; d--)
                    {
                        int toDropIndex = PW[PW.Count - d].DeformerIndex;
                        InternalValues.SetValue(0, toDropIndex, i);
                        removed++;
                    }
                }
            }

            return removed;
        }

        public CG_PointData ShareFields(CG_PointData inData)
        {
            //Create a full list of Fields
            List<string> fields = Fields;
            Dictionary<string, object[]> belongs = new Dictionary<string, object[]>();

            int counter = 0;
            foreach (string str in inData.Fields)
            {
                if (!fields.Contains(str))
                {
                    fields.Add(str);
                    belongs.Add(str, new object[] {inData, counter});
                }
                else
                {
                    belongs.Add(str, new object[] { this, fields.IndexOf(str) });
                }
                counter++;
            }

            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), fields.Count, PointCount), fields, Type);

            for (int i = 0; i < fields.Count; i++)
            {
                object[] fieldInfos = belongs[fields[i]];
                CG_PointData owner = fieldInfos[0] as CG_PointData;

                for (int j = 0; j < PointCount; j++)
                {
                    modified.InternalValues.SetValue(owner.InternalValues.GetValue((int)fieldInfos[1], j), i, j);
                }
            }

            return modified;
        }

        public CG_PointData TransposeFields(List<string> inFields)
        {
            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), inFields.Count, PointCount), new List<string>() , Type);
            
            int index;

            for (int i = 0; i < inFields.Count; i++)
            {
                modified.Fields.Add(inFields[i]);
                index = Fields.IndexOf(inFields[i]);
                
                if(index == -1)
                {
                    index = i;
                }

                for (int j = 0; j < PointCount; j++)
                {
                    modified.InternalValues.SetValue(InternalValues.GetValue(i, j), index, j);
                }
            }

            return modified;

        }

        public CG_PointData Blend(CG_PointData inData, double Opacity)
        {
            return Blend(inData, Opacity, null);
        }

        public CG_PointData Blend(CG_PointData Merged, double Opacity, List<int> indices)
        {
            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), Fields.Count, PointCount), new List<string>(), Type);

            for (int i = 0; i < Fields.Count; i++)
            {
                modified.Fields.Add(Fields[i]);

                for (int j = 0; j < PointCount; j++)
                {
                    if (indices == null ||indices.Count == 0 || indices.Contains(j))
                    {
                        modified.InternalValues.SetValue((double)InternalValues.GetValue(i, j) * (1 - Opacity) + (double)Merged.InternalValues.GetValue(i, j) * Opacity, i, j);
                    }
                    else
                    {
                        modified.InternalValues.SetValue((double)InternalValues.GetValue(i, j), i, j);
                    }
                }
            }

            return modified;
        }

        public CG_PointData PutValue(double Value, List<string> inFields, bool ExceptGivenFields)
        {
            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), Fields.Count, PointCount), new List<string>(), Type);
            
            for (int i = 0; i < Fields.Count; i++)
            {
                modified.Fields.Add(Fields[i]);
                if (inFields != null && inFields.Count > 0)
                {
                    if (inFields.Contains(Fields[i]) == !ExceptGivenFields)
                    {
                        for (int j = 0; j < PointCount; j++)
                        {
                            modified.InternalValues.SetValue(Value, i, j);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < PointCount; j++)
                        {
                            modified.InternalValues.SetValue(InternalValues.GetValue(i, j), i, j);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < PointCount; j++)
                    {
                        modified.InternalValues.SetValue(Value, i, j);
                    }
                }
            }

            return modified;
        }

        public string FindStaticField(List<int> Indices, double Value)
        {
            bool Equals = true;
            for (int i = 0; i < Fields.Count; i++)
            {
                for (int j = 0; j < PointCount; j++)
                {
                    if(Indices.Contains(j))
                    {
                        if (!TypesHelper.DoubleIsFuzzyEqual((double)InternalValues.GetValue(i, j), Value, EPSILON))
                        {
                            Equals = false;
                            break;
                        }
                    }
                }

                if (Equals)
                {
                    return Fields[i];
                }

                Equals = true;
            }

            return string.Empty;
        }

        public CG_PointData FilterFields(List<string> deformers)
        {
            CG_PointData modified = new CG_PointData(Array.CreateInstance(typeof(object), deformers.Count, PointCount), new List<string>(), Type);

            //Get deformers
            List<string> notFoundFields = new List<string>();

            //ignore deformers that don't exist in ref deformers
            int counter = 0;
            foreach (string defName in Fields)
            {
                if (deformers.Contains(defName))
                {
                    modified.Fields.Add(defName);
                    for (int j = 0; j < PointCount; j++)
                    {
                        modified.InternalValues.SetValue(InternalValues.GetValue(counter, j), modified.Fields.Count - 1, j);
                    }
                }

                counter++;
            }

            //Add deformers that don't exist in old deformers
            foreach (string defName in deformers)
            {
                if (!modified.Fields.Contains(defName))
                {
                    modified.Fields.Add(defName);
                    for (int j = 0; j < PointCount; j++)
                    {
                        modified.InternalValues.SetValue(0.0, modified.Fields.Count - 1, j);
                    }
                }
            }

            return modified;
        }

        public void Update(CG_PointData curData)
        {
            int index;

            for (int i = 0; i < Fields.Count; i++)
            {
                index = curData.Fields.IndexOf(Fields[i]);

                if (index != -1)
                {
                    for (int j = 0; j < PointCount; j++)
                    {
                        InternalValues.SetValue(curData.InternalValues.GetValue(index, j), i, j);
                    }
                }
            }
        }
    }
}
