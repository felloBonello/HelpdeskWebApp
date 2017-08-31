using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using HelpdeskDAL;
using System.Diagnostics;

namespace HelpdeskViewModels
{
    public class ViewModelUtils
    {
        /// <summary>
        /// Common ViewModel Error Method
        /// </summary>
        /// <param name="e">Exception thrown</param>
        /// <param name="obj">Class throwing exception</param>
        /// <param name="method">Method throwing exception</param>
        public static void ErrorRoutine(Exception e, string obj, string method)
        {
            if (e.InnerException != null)
            {
                Trace.WriteLine("Error in ViewModel, object=" + obj +
                                                ", method=" + method +
                                                " , inner exception message=" +
                                                e.InnerException.Message);
                throw e.InnerException;
            }
            else
            {
                Trace.WriteLine("Error in ViewModel, object=" + obj +
                                                ", method=" + method + " . message=" +
                                                e.Message);
                throw e;
            }
        }

        ///<summary>
        /// LoadCollections
        ///             trigger DAL to remove existing collections and then re-load them
        /// <returns>indicator if collections created</returns>
        /// </summary>
        public bool LoadCollections()
        {
            bool createOk = false;

            try
            {
                DALUtils dalUtil = new DALUtils();
                createOk = dalUtil.LoadCollections();
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ViewModelUtils", "LoadCollections");
            }

            return createOk;
        }

        /// <summary>
        /// Serializer
        /// </summary>
        /// <param name="inObject">Object to be serialized</param>
        /// <returns>Serialized Object in byte array format</returns>
        public static byte[] Serializer(Object inObject)
        {
            byte[] byteArrayObject;
            BinaryFormatter frm = new BinaryFormatter();
            MemoryStream strm = new MemoryStream();
            frm.Serialize(strm, inObject);
            byteArrayObject = strm.ToArray();
            return byteArrayObject;
        }

        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns>Deserialized Object</returns>
        public static Object Deserializer(byte[] byteArrayIn)
        {
            BinaryFormatter frm = new BinaryFormatter();
            MemoryStream strm = new MemoryStream(byteArrayIn);
            return frm.Deserialize(strm);
        }
    }
}