using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class Form : Dictionary<string, string>
    {
        #region Lấy giá trị
		public T layDTO<T>(string key, T macDinh = null)
            where T : DTOLayer.DTO, new()
        {
            try
            {
                int? maTam = layInt(key);

                return maTam.HasValue ?
                    new T()
                    {
                        ma = maTam
                    } :
                    macDinh;
            }
            catch
            {
                return macDinh;
            }
        }

        public string layString(string key, string macDinh = null)
        {
            try
            {
                return this[key];
            }
            catch
            {
                return macDinh;
            }
        }

        public int? layInt(string key, int? macDinh = null)
        {
            try
            {
                return int.Parse(this[key]);
            }
            catch
            {
                return macDinh;
            }
        }

        public DateTime? layDate(string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.ParseExact(this[key], "d/M/yyyy", null);
            }
            catch
            {
                return macDinh;
            }
        }

        public DateTime? layTime(string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.ParseExact(this[key], "H:mm", null);
            }
            catch
            {
                return macDinh;
            }
        }

        public DateTime? layDateTime(string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.ParseExact(this[key], "H:mm d/M/yyyy", null);
            }
            catch
            {
                return macDinh;
            }
        }

        public bool layBool(string key, bool macDinh = false)
        {
            try
            {
                return this[key] == "1";
            }
            catch
            {
                return macDinh;
            }
        }  
	    #endregion
    }
}
