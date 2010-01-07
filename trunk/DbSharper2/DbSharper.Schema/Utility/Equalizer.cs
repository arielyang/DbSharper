using System;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

namespace DbSharper.Schema.Utility
{
	public static class Equalizer
	{
		#region Methods

		public static bool IsEqual(object source, object target)
		{
			if (source == null || target == null)
			{
				return source == target;
			}

			Type type = source.GetType();

			if (type != target.GetType())
			{
				return false;
			}
			else if (type.GetInterface("System.IEquatable`1") != null)
			{
				return source.Equals(target);
			}
			else if (type.GetInterface("System.Collections.IEnumerable") != null)
			{
				if (!CompareIEnumerableObjects(source, target))
				{
					return false;
				}
			}

			if (!CompareFields(type, source, target))
			{
				return false;
			}

			if (!ComapreProperties(type, source, target))
			{
				return false;
			}

			return true;
		}

		private static bool ComapreProperties(Type type, object source, object target)
		{
			PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				// Is not indexed property.
				if (propertyInfo.GetIndexParameters().Length == 0)
				{
					// Has not XmlIgnoreAttribute.
					if (propertyInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length == 0)
					{
						// Compare property value.
						if (!IsEqual(propertyInfo.GetValue(source, null), propertyInfo.GetValue(target, null)))
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		private static bool CompareFields(Type type, object source, object target)
		{
			FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				// Has not XmlIgnoreAttribute.
				if (fieldInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length == 0)
				{
					if (!IsEqual(fieldInfo.GetValue(source), fieldInfo.GetValue(target)))
					{
						return false;
					}
				}
			}

			return true;
		}

		private static bool CompareIEnumerableObjects(object source, object target)
		{
			IEnumerator sourceEnumerator = (source as IEnumerable).GetEnumerator();
			IEnumerator targetEnumerator = (target as IEnumerable).GetEnumerator();

			bool loop = false;

			do
			{
				bool sourceMoveNext = sourceEnumerator.MoveNext();
				bool targetMoveNext = targetEnumerator.MoveNext();

				if (sourceMoveNext != targetMoveNext) // Length is different.
				{
					return false;
				}

				if (sourceMoveNext/* targetMoveNext */)
				{
					if (!IsEqual(sourceEnumerator.Current, targetEnumerator.Current))
					{
						return false;
					}

					loop = true;
				}
				else
				{
					loop = false;
				}

			}
			while (loop);

			return true;
		}

		#endregion Methods
	}
}