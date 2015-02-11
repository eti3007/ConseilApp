using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConseilDAL.Exceptions
{
    public class CustomException
    {
        public DbEntityValidationException CustomValidationExceptionReturn(DbEntityValidationException ex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }
            return new DbEntityValidationException(
                        "Entity Validation Failed - errors follow:\n" +
                        sb.ToString(), ex
                    ); // Add the original exception as the innerException
        }

        public Exception CustomGetException(Exception exc)
        {
            Exception ex = new Exception("Erreur de récupération de la liste des styles.");
            ex.Data.Add("funcName", "GetList");
            ex.Data.Add("originalException", exc);

            return ex;
        }
    }
}
