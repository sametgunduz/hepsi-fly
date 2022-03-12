namespace HepsiFly.Common.Exceptions;

public interface IHepsiFlyException
    {
        
    }
    
    public class HepsiFlyException: Exception,IHepsiFlyException
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Code { get; set; }
        public new string StackTrace { get; set; }
        public List<string> Errors { get; set; }
    }
    
     public class HepsiFlyExceptions
    {
        #region Http Status Code 400

        public static HepsiFlyException GetValidationException(List<string> errors)
        {
            return new HepsiFlyException
            {
                Code = "NJgyXb",
                Title = "Validation error",
                Errors = errors
            };
        }
        
        public static readonly HepsiFlyException ExternalServiceException = new()
        {
            Code = "fC9LZX",
            Title = "External service error",
            Detail = "The external service error occurred.",
        };

        public static readonly HepsiFlyException BadRequest = new()
        {
            Code = "fC9LZX",
            Title = "Invalid request parameters",
            Detail = "The provided resource parameters are not valid.",
        };

        public static readonly HepsiFlyException InvalidRequestBody = new()
        {
            Code = "cqE9QM",
            Title = "Invalid request body",
            Detail = "The provided resource body is not valid."
        };

        public static readonly HepsiFlyException UniqueConstraintViolation = new()
        {
            Code = "FFj5ca",
            Title = "Unique Constraint Violation",
            Detail = "Unique Constraint Violation."
        };
        #endregion

        #region Http Status Code 402

        public static readonly HepsiFlyException PaymentRequired = new()
        {
            Code = "i0HGQJ",
            Title = "Subscription Failure",
            Detail = "In order to use this functionality, user should have active subscription."
        };

        #endregion

        #region Http Status Code 403

        public static readonly HepsiFlyException Forbidden = new()
        {
            Code = "23brAV",
            Title = "Forbidden",
            Detail = "You are not authorized to take this action."
        };

        public static readonly HepsiFlyException SelfRequestForbidden = new()
        {
            Code = "PquMRW",
            Title = "Forbidden",
            Detail = "You can not send any kind of request to yourself."
        };

        #endregion

        #region Http Status Code 404

        public static readonly HepsiFlyException NotFoundException = new()
        {
            Code = "enGL8L",
            Title = "Not Found",
            Detail = "The underlying resource could not be found with provided data."
        };
        
        public static readonly HepsiFlyException CategoryNotFound = new()
        {
            Code = "a2C2ha",
            Title = "Not Found",
            Detail = "Category not found."
        };
        #endregion

        #region Http Status Code 406

        public static HepsiFlyException GetMissingHeaderError(string key)
        {
            return new HepsiFlyException
            {
                Code = "U2A2bQ",
                Title = $"Missing header '{key}'",
                Detail = $"The '{key}' header is required.",
            };
        }

        #endregion

        #region Http Status Code 409

        public static readonly HepsiFlyException UserAlreadyRegistered = new()
        {
            Code = "Jn2kw9",
            Title = "User Registration Error",
            Detail = "User has been registered before."
        };
        
        public static readonly HepsiFlyException DeviceAlreadyRegistered = new()
        {
            Code = "Jn2kw9",
            Title = "Device Registration Error",
            Detail = "Device has been registered before."
        };

        public static readonly HepsiFlyException PhoneNumberExists = new()
        {
            Code = "pMdQdN",
            Title = "Phone Number Error",
            Detail = "The provided phone number is not available."
        };


        #endregion

        #region Http Status Code 424

        public static readonly HepsiFlyException UnregisteredDevice = new()
        {
            Code = "UXNqTK",
            Title = "Unregistered Device",
            Detail = "Unregistered Device."
        };

        #endregion

        #region Http Status Code 5xx

        public static readonly HepsiFlyException InternalServerException = new()
        {
            Code = "qQfK7V",
            Title = "Unexpected internal error occured",
            Detail = "Unexpected internal error occured"
        };
        
        public static readonly HepsiFlyException VerificationFailed = new()
        {
            Code = "qQfK7V",
            Title = "Unexpected internal error occured",
            Detail = "Verification failed."
        };

        public static HepsiFlyException GetGenericException(string title, string message)
        {
            return new HepsiFlyException
            {
                Code = "Az35ka",
                Title = title,
                Detail = message
            };
        }

        #endregion
    }