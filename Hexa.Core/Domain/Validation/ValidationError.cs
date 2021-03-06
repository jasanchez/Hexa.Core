using System;

namespace Hexa.Core.Validation
{
    /// <summary>
    /// Details of a validation error
    /// </summary>
    public struct ValidationError : IValidationError
	{
		#region fields
		private readonly Type _entityType;
		private readonly string _propertyName;
		private readonly string _message;
		#endregion

		#region properties
		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <value>The type of the entity.</value>
		public Type EntityType { get { return _entityType; } }
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName { get { return _propertyName; } }
		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get { return _message; } }
		#endregion

		#region ctor
		/// <summary>
		/// Default Constructor.
		/// Creates a new instance of the <see cref="ValidationError"/> data structure.
		/// </summary>
		/// <param name="message">string. The validation error message.</param>
		/// <param name="property">string. The property that was validated.</param>
		// TODO: Swap parameter order.
		public ValidationError(string message, string property)
		{
			Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(message),
												 "Please provide a valid non null string as the validation error message");
			Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(property),
												 "Please provide a valid non null string as the validation property name");
			
			_entityType = typeof(void); // Avoid make this.EntityType == null as to not breaking existing code.
			_message = message;
			_propertyName = property;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ValidationError"/> data structure.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="message">string. The validation error message.</param>
		/// <param name="property">string. The property that was validated.</param>
		// TODO: Swap parameter order so property comes before message.
		public ValidationError(Type entityType, string message, string property)
        {
			Guard.Against<ArgumentNullException>(entityType == null,
									 "Please provide a valid non null Type as the validated Entity");
			Guard.Against<ArgumentNullException>(string.IsNullOrEmpty("message"),
                                                 "Please provide a valid non null string as the validation error message");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty("property"),
                                                 "Please provide a valid non null string as the validation property name");
			_entityType = entityType;
			_message = message;
            _propertyName = property;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationError"/> struct.
		/// </summary>
		/// <param name="ex">The exception which caused the validation error.</param>
		public ValidationError(string property, Exception ex)
			: this(ex.Message, property)
		{
		}
        #endregion

        #region methods
        /// <summary>
        /// Overriden. Gets a string that represents the validation error.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}::{1}) - {2}", EntityType.ToString(), PropertyName, Message);
        }

        /// <summary>
        /// Overridden. Compares if an object is equal to the <see cref="ValidationError"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof (ValidationError)) return false;
            return Equals((ValidationError) obj);
        }

        /// <summary>
        /// Overriden. Compares if a <see cref="ValidationError"/> instance is equal to this
        /// <see cref="ValidationError"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(IValidationError obj)
        {
            return Equals(obj.EntityType, EntityType)
				&& Equals(obj.PropertyName, PropertyName)
				&& Equals(obj.Message, Message);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Message.GetHashCode() * 397) ^ PropertyName.GetHashCode();
            }
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ValidationError left, ValidationError right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ValidationError left, ValidationError right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}