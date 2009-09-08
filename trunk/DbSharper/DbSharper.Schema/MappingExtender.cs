//namespace DbSharper.Schema
//{
//    using System.Data;

//    using DbSharper.Schema.Code;
//    using DbSharper.Schema.Collections;
//    using DbSharper.Schema.Enums;
//    using System.Collections.Generic;
//    using System.Text;
//    using System;

//    public class MappingExtender
//    {
//        #region Fields

//        private Mapping mapping;

//        #endregion Fields

//        #region Constructors

//        public MappingExtender(Mapping mapping)
//        {
//            this.mapping = mapping;
//        }

//        #endregion Constructors

//        #region Delegates

//        private delegate Method ExtendDelegate(Model model);

//        #endregion Delegates

//        #region Methods

//        public void Extend()
//        {
//            // Extend all namespaces.
//            foreach (var ns in mapping.ModelNamespaces)
//            {
//                // Extend for all models.
//                foreach (var model in ns.Models)
//                {
//                    //ExtendMethod(model, "Create", ExtendCreateMethod);
//                    //ExtendMethod(model, "Update", ExtendUpdateMethod);
//                    ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // GetItem
//                    ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // GetList
//                    ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // GetListByIds
//                    ExtendMethod(model, ExtendDeleteByPrimaryKeyMethod); // Delete
//                    ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // DeleteByIds
//                }
//            }
//        }

//        private Method ExtendDeleteByPrimaryKeyMethod(Model model)
//        {
//            Method method = new Method
//            {
//                Name = "Delete",
//                Description = string.Format("Delete {0} item by primary key.", model.Name),
//                CommandType = CommandType.Text,
//                MethodType = MethodType.ExecuteReader
//            };

//            // Add parameters.
//            foreach (var property in model.Properties)
//            {
//                if (property.IsPrimaryKey)
//                {
//                    method.Parameters.Add(PropertyToParameter(property));
//                }
//            }

//            // Add result.
//            method.Results.Add(
//                new Result
//                {
//                    Name = "ReturnResult",
//                    CommonType = "ReturnResult",
//                    Description = "Return result.",
//                    IsOutputParameter = true
//                });

//            return method;
//        }

//        private Method ExtendCreateMethod(Model model)
//        {
//            if (model.IsView)
//            {
//                return null;
//            }
//            NamedCollection<Parameter> parameters = new NamedCollection<Parameter>();
//            foreach (var property in model.Properties)
//            {
//                parameters.Add(PropertyToParameter(property));
//            }
//            Method method = new Method
//                {
//                    Name = "Create",
//                    Description = string.Format("Create a new {0}.", model.Name),
//                    CommandType = CommandType.Text,
//                    MethodType = MethodType.ExecuteNonQuery,
//                    CommandText = "",
//                    Parameters = parameters
//                };
//            if (HasSinglePrimaryKeyProperty(model)) // Only single primary key needs a returned inserted primary key.
//            {
//                Property property = GetSinglePriamryKeyProperty(model);
//                Parameter parameter = PropertyToParameter(property);
//                parameter.Name = "Inserted" + property.Name;
//                parameter.SqlName = "@Inserted" + property.Name;
//                parameter.Direction = ParameterDirection.InputOutput;
//                method.Parameters.Add(parameter);
//                method.Results.Add(PropertyToResult(property));
//            }
//            return method;
//        }

//        private Method ExtendGetItemByPrimaryKeyMethod(Model model)
//        {
//            if (model.IsView)
//            {
//                return null;
//            }

//            Method method = new Method
//            {
//                Name = "GetItem",
//                Description = string.Format("Get {0} item by primary key.", model.Name),
//                CommandType = CommandType.Text,
//                MethodType = MethodType.ExecuteReader
//            };

//            // Add parameters.
//            foreach (var property in model.Properties)
//            {
//                if (property.IsPrimaryKey)
//                {
//                    method.Parameters.Add(PropertyToParameter(property));
//                }
//            }

//            // Add result.
//            method.Results.Add(
//                new Result
//                {
//                    Name = model.Name + "Item",
//                    CommonType = model.Name + "Item",
//                    Description = model.Name + " Item.",
//                    IsOutputParameter = false
//                });

//            StringBuilder sb = new StringBuilder();

//            sb.AppendLine("SELECT");
//            sb.AppendLine("FROM");

//            foreach (var property in model.Properties)
//            {
//                if (property.ReferenceType != property.Type.ToString())
//                {

//                }
//            }

//            sb.AppendLine();
//            sb.AppendLine("WHERE");

//            foreach (var property in model.Properties)
//            {
//                if (property.IsPrimaryKey)
//                {
//                    sb.AppendFormat("\t{0}.{1} = @{1} AND", model.MappingSource, property.Name);
//                    sb.AppendLine();
//                }
//            }

//            sb.Length = sb.Length - " AND".Length - Environment.NewLine.Length;

//            sb.AppendLine("\t{0} = @{0}");

//            return method;
//        }

//        /// <summary>
//        /// Extend a method for specific model.
//        /// </summary>
//        /// <param name="model">Model to extend.</param>
//        /// <param name="extendMethod">Extend method delegate.</param>
//        private void ExtendMethod(Model model, ExtendDelegate extendMethod)
//        {
//            Method method = extendMethod.Invoke(model);

//            if (method == null)
//            {
//                return;
//            }

//            DataAccessNamespace dataAccessNamespace;

//            DataAccess dataAccess;

//            // Discover relative DataAccessNamespace or create a new one if does not exist.
//            if (mapping.DataAccessNamespaces.Contains(model.Schema))
//            {
//                dataAccessNamespace = mapping.DataAccessNamespaces[model.Schema];
//            }
//            else
//            {
//                dataAccessNamespace = new DataAccessNamespace { Name = model.Schema };

//                mapping.DataAccessNamespaces.Add(dataAccessNamespace);
//            }

//            // Discover relative DataAccess or create a new one if does not exist.
//            if (dataAccessNamespace.DataAccesses.Contains(model.Name))
//            {
//                dataAccess = dataAccessNamespace.DataAccesses[model.Name];
//            }
//            else
//            {
//                dataAccess = new DataAccess
//                {
//                    Name = model.Name,
//                    Schema = model.Schema,
//                    Description = string.Format("Auto generated class for model {0}.", model.Name)
//                };

//                dataAccessNamespace.DataAccesses.Add(dataAccess);
//            }

//                // Create a method for model.
//                if (!dataAccess.Methods.Contains(method.Name))
//                {
//                    dataAccess.Methods.Add(method);
//                }
//        }

//        private Method ExtendUpdateMethod(Model model)
//        {
//            if (model.IsView)
//            {
//                return null;
//            }
//            NamedCollection<Parameter> parameters = new NamedCollection<Parameter>();
//            foreach (var property in model.Properties)
//            {
//                if (!property.HasDefault)
//                {
//                    parameters.Add(PropertyToParameter(property));
//                }
//            }
//            Method method = new Method
//            {
//                Name = "Update",
//                Description = string.Format("Update a {0}.", model.Name),
//                CommandType = CommandType.Text,
//                MethodType = MethodType.ExecuteNonQuery,
//                CommandText = "",
//                Parameters = parameters
//            };
//            method.Results.Add(
//                new Result
//                {
//                    Name = "ReturnResult",
//                    Description = "ReturnResult",
//                    IsOutputParameter = false,
//                    CommonType = "ReturnResult"
//                });
//            return method;
//        }

//        private List<Property> GetSinglePriamryKeyProperty(Model model)
//        {
//            List<Property> list = new List<Property>();

//            foreach (var property in model.Properties)
//            {
//                if (property.IsPrimaryKey)
//                {
//                    list.Add(property);
//                }
//            }

//            return list;
//        }

//        private bool HasSinglePrimaryKeyProperty(Model model)
//        {
//            int count = 0;

//            foreach (var property in model.Properties)
//            {
//                if (property.IsPrimaryKey)
//                {
//                    count++;
//                }
//            }

//            return count == 1;
//        }

//        /// <summary>
//        /// Transform a model property to a method parameter.
//        /// </summary>
//        /// <param name="property">Model property.</param>
//        /// <returns>Method paramter.</returns>
//        private Parameter PropertyToParameter(Property property)
//        {
//            return new Parameter
//                {
//                    Name = property.Name,
//                    Description = property.Description,
//                    Direction = ParameterDirection.Input,
//                    Size = property.Size,
//                    SqlDbType = property.SqlDbType,
//                    SqlName = "@" + property.Name,
//                    Type = property.Type
//                };
//        }

//        /// <summary>
//        /// Transform a primary key model property to an inserted result.
//        /// </summary>
//        /// <param name="property">Primary key model property.</param>
//        /// <returns>Inserted result.</returns>
//        private Result PropertyToResult(Property property)
//        {
//            return new Result
//                {
//                    Name = "Inserted" + property.Name,
//                    Description = "Inserted " + property.Name,
//                    IsOutputParameter = true,
//                    CommonType = property.Type.ToString()
//                };
//        }

//        #endregion Methods
//    }
//}