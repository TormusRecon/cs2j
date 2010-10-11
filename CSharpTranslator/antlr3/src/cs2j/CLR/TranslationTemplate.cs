using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using RusticiSoftware.Translator.Utils;

// These Template classes are in-memory versions of the xml translation templates
// (we use C# to directly persist to / from files).

// We have overloaded Equals to test value equality for these template objects.  For now its only 
// used by unit tests (to check that the object survives xml serialization / deserialization
// unscathed). But it might be useful down the road.
// By overloading Equals, we also have to overload GetHashcode (well, its highly reccomended)...

namespace RusticiSoftware.Translator.CLR
{

	// Simple <type> <name> pairs to represent formal parameters
	public class ParamRepTemplate : IEquatable<ParamRepTemplate>
	{
		public string Type { get; set; }
		public string Name { get; set; }

		public ParamRepTemplate ()
		{
		}

		public ParamRepTemplate (string t, string a)
		{
			Type = t;
			Name = a;
		}

		#region Equality
		public bool Equals (ParamRepTemplate other)
		{
			if (other == null)
				return false;
			
			return Type == other.Type && Name == other.Name;
		}

		public override bool Equals (object obj)
		{
			
			ParamRepTemplate temp = obj as ParamRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (ParamRepTemplate a1, ParamRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (ParamRepTemplate a1, ParamRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (Type ?? String.Empty).GetHashCode () ^ (Name ?? String.Empty).GetHashCode ();
		}
		#endregion
	}

		// A use entry.  An optional alias and a fully qualified namespace
	public class UseRepTemplate : IEquatable<UseRepTemplate>
	{

		public string Alias { get; set; }
		public string Namespace { get; set; }


		public UseRepTemplate () : this(null)
		{
		}

		public UseRepTemplate (string u) : this(null, u)
		{
		}

		public UseRepTemplate (string a, string u)
		{
			Alias = a;
			Namespace = u;
		}

		#region Equality
		public bool Equals (UseRepTemplate other)
		{
			if (other == null)
				return false;
			
			return Alias == other.Alias && Namespace == other.Namespace;
		}

		public override bool Equals (object obj)
		{
			
			UseRepTemplate temp = obj as UseRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (UseRepTemplate a1, UseRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (UseRepTemplate a1, UseRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (Alias ?? String.Empty).GetHashCode () ^ (Namespace ?? String.Empty).GetHashCode ();
		}
		#endregion
		
	}


	// Never directly create a TranslationBase. Its a common root for translatable language entities
	public abstract class TranslationBase : IEquatable<TranslationBase>
	{
		[XmlArrayItem("Import")]
		// Java imports required to make Java translation run
		public string[] Imports { get; set; }
		// The Java translation for this C# entity
		public string Java { get; set; }

		protected TranslationBase ()
		{
			Imports = null;
			Java = null;
		}

		protected TranslationBase (string java)
		{
			Imports = null;
			Java = java;
		}

		protected TranslationBase (string[] imps, string java)
		{
			Imports = imps;
			Java = java;
		}

		#region Equality

		public bool Equals (TranslationBase other)
		{
			if (other == null)
				return false;
			
			if (Imports != other.Imports) {
				if (Imports == null || other.Imports == null || Imports.Length != other.Imports.Length)
					return false;
				for (int i = 0; i < Imports.Length; i++) {
					if (Imports[i] != other.Imports[i])
						return false;
				}
			}
			
			return Java == other.Java;
		}

		public override bool Equals (object obj)
		{
			
			TranslationBase temp = obj as TranslationBase;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (TranslationBase a1, TranslationBase a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (TranslationBase a1, TranslationBase a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = 0;
			if (Imports != null) {
				foreach (string e in Imports) {
					hashCode ^= e.GetHashCode();
				}
			}
			return (Java ?? String.Empty).GetHashCode () ^ hashCode;
		}
		#endregion
		
	}


	public class ConstructorRepTemplate : TranslationBase, IEquatable<ConstructorRepTemplate>
	{

		private List<ParamRepTemplate> _params = null;
		[XmlArrayItem("Param")]
		public List<ParamRepTemplate> Params {
			get {
				if (_params == null)
					_params = new List<ParamRepTemplate> ();
				return _params;
			}
		}

		public ConstructorRepTemplate () : base()
		{
		}

		public ConstructorRepTemplate (List<ParamRepTemplate> pars) : base()
		{
			_params = pars;
		}

		public ConstructorRepTemplate (List<ParamRepTemplate> pars, string[] imps, string javaRep) : base(imps, javaRep)
		{
			_params = pars;
		}


		#region Equality

		public bool Equals (ConstructorRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Params != other.Params) {
				if (Params == null || other.Params == null || Params.Count != other.Params.Count)
					return false;
				for (int i = 0; i < Params.Count; i++) {
					if (Params[i] != other.Params[i])
						return false;
				}
			}

			return base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			ConstructorRepTemplate temp = obj as ConstructorRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (ConstructorRepTemplate a1, ConstructorRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (ConstructorRepTemplate a1, ConstructorRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = 0;
			foreach (ParamRepTemplate o in Params) {
				hashCode = hashCode ^ o.GetHashCode() ;
			}
			
			return base.GetHashCode () ^ hashCode;
		}
		#endregion
	}

	// Method has the same info as a delegate as a constructor plus a name and return type
	public class MethodRepTemplate : ConstructorRepTemplate, IEquatable<ConstructorRepTemplate>
	{
		// Method name
		public string Name { get; set; }

		// Return type
		public string Return { get; set; }

		public MethodRepTemplate ()
		{
		}

		public MethodRepTemplate (string retType, string methodName, List<ParamRepTemplate> pars, string[] imps, string javaRep) : base(pars, imps, javaRep)
		{
			Name = methodName;
			Return = retType;
		}

		public MethodRepTemplate (string retType, string methodName, List<ParamRepTemplate> pars) : this(retType, methodName, pars, null, null)
		{
		}

		#region Equality
		public bool Equals (MethodRepTemplate other)
		{
			if (other == null)
				return false;
			
			return Return == other.Return && Name == other.Name && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			MethodRepTemplate temp = obj as MethodRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (MethodRepTemplate a1, MethodRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (MethodRepTemplate a1, MethodRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (Return ?? String.Empty).GetHashCode () ^ (Name ?? String.Empty).GetHashCode () ^ base.GetHashCode();
		}
		#endregion

	}

	//  A user-defined cast from one type to another
	public class CastRepTemplate : TranslationBase, IEquatable<CastRepTemplate>
	{
		// From and To are fully qualified types
		public string From { get; set; }
		public string To { get; set; }

		public CastRepTemplate () : base()
		{
		}

		public CastRepTemplate (string fType, string tType, string[] imps, string java) : base(imps, java)
		{
			From = fType;
			To = tType;
		}

		public CastRepTemplate (string fType, string tType) : this(fType, tType, null, null)
		{
		}

		#region Equality
		public bool Equals (CastRepTemplate other)
		{
			if (other == null)
				return false;
			
			return From == other.From && To == other.To && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			CastRepTemplate temp = obj as CastRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (CastRepTemplate a1, CastRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (CastRepTemplate a1, CastRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (From ?? String.Empty).GetHashCode() ^ (To ?? String.Empty).GetHashCode() ^ base.GetHashCode();
		}
		#endregion
		
	}

	// A member field definition
	public class FieldRepTemplate : TranslationBase, IEquatable<FieldRepTemplate>
	{

		public string Type { get; set; }
		public string Name { get; set; }

		public FieldRepTemplate () : base()
		{
		}

		public FieldRepTemplate (string fType, string fName, string[] imps, string javaGet) : base(imps, javaGet)
		{
			Type = fType;
			Name = fName;
		}

		public FieldRepTemplate (string fType, string fName) : this(fType, fName, null, null)
		{
		}

		#region Equality
		public bool Equals (FieldRepTemplate other)
		{
			if (other == null)
				return false;
			
			return Type == other.Type && Name == other.Name && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			FieldRepTemplate temp = obj as FieldRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (FieldRepTemplate a1, FieldRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (FieldRepTemplate a1, FieldRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (Type ?? String.Empty).GetHashCode() ^ (Name ?? String.Empty).GetHashCode() ^ base.GetHashCode();
		}
		#endregion
		
	}

	// A property definition.  We need separate java translations for getter and setter.  If JavaGet is null
	// then we can use Java (from TranslationBase) as the translation for gets.
	// Imports are shared between getter and setter (might lead to some unneccessary extra imports, but I'm
	// guessing that normally imports will be the same for both)
	public class PropRepTemplate : FieldRepTemplate, IEquatable<PropRepTemplate>
	{
		public string JavaSet { get; set; }
		private string _javaGet = null;
		public string JavaGet {
			get { return _javaGet ?? Java; }
			set { _javaGet = value; }
		}

		public PropRepTemplate () : base()
		{
		}

		public PropRepTemplate (string fType, string fName, string[] imps, string javaGet, string javaSet) : base(fType, fName, imps, null)
		{
			JavaGet = javaGet;
			JavaSet = javaSet;
		}

		public PropRepTemplate (string fType, string fName) : this(fType, fName, null, null, null)
		{
		}

		
		#region Equality
		public bool Equals (PropRepTemplate other)
		{
			if (other == null)
				return false;
			
			return JavaGet == other.JavaGet && JavaSet == other.JavaSet && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			PropRepTemplate temp = obj as PropRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (PropRepTemplate a1, PropRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (PropRepTemplate a1, PropRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (JavaGet ?? String.Empty).GetHashCode () ^ (JavaSet ?? String.Empty).GetHashCode () ^ base.GetHashCode ();
		}
		#endregion		
	}

	// A member of an enum,  may also have a numeric value
	public class EnumMemberRepTemplate : TranslationBase, IEquatable<EnumMemberRepTemplate>
	{

		public string Name { get; set; }
		public string Value { get; set; }


		public EnumMemberRepTemplate () : this(null)
		{
		}

		public EnumMemberRepTemplate (string n) : this(n, null, null, null)
		{
		}

		public EnumMemberRepTemplate (string n, string v) : this(n, v, null, null)
		{
		}

		public EnumMemberRepTemplate (string n, string v, string[] imps, string java) : base(imps, java)
		{
			Name = n;
			Value = v;
		}

		#region Equality
		public bool Equals (EnumMemberRepTemplate other)
		{
			if (other == null)
				return false;
			
			return Name == other.Name && Value == other.Value && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			EnumMemberRepTemplate temp = obj as EnumMemberRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (EnumMemberRepTemplate a1, EnumMemberRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (EnumMemberRepTemplate a1, EnumMemberRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return (Name ?? String.Empty).GetHashCode () ^ (Value ?? String.Empty).GetHashCode () ^ base.GetHashCode ();
		}
		#endregion		
		
	}



	// Base Template for classes, interfaces, enums, etc.
	[Serializable]
	public abstract class TypeRepTemplate : TranslationBase, IEquatable<TypeRepTemplate>
	{
		// Type Name
		[XmlElementAttribute("Name")]
		public string TypeName { get; set; }

		// Path to use when resolving types
		[XmlElementAttribute("Use")]
		public UseRepTemplate[] Uses { get; set; }

		public TypeRepTemplate () : base()
		{
			TypeName = null;
			Uses = null;
			
		}

		protected TypeRepTemplate (string typeName) : this()
		{
			TypeName = typeName;
		}

		protected TypeRepTemplate (string tName, UseRepTemplate[] usePath, string[] imports, string javaTemplate) : base(imports, javaTemplate)
		{
			TypeName = tName;
			Uses = usePath;
		}

		private static object Deserialize (Stream fs, System.Type t)
		{
			object o = null;
			
			XmlSerializer serializer = new XmlSerializer (t, Constants.TranslationTemplateNamespace);
			o = serializer.Deserialize (fs);
			return o;
		}


		private static TypeRepTemplate Deserialize (Stream s)
		{
			TypeRepTemplate ret = null;
			
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			XmlReader reader = XmlReader.Create (s, settings);
			
			//XmlTextReader reader = new XmlTextReader(s);
			string typeType = null;
			// class, interface, enum, etc.
			bool found = false;
			
			try {
				while (reader.Read () && !found) {
					if (reader.NodeType == XmlNodeType.Element) {
						switch (reader.LocalName) {
						case "Class":
							typeType = "RusticiSoftware.Translator.CLR.ClassRepTemplate";
							break;
						case "Struct":
							typeType = "RusticiSoftware.Translator.CLR.StructRepTemplate";
							break;
						case "Interface":
							typeType = "RusticiSoftware.Translator.CLR.InterfaceRepTemplate";
							break;
						case "Enum":
							typeType = "RusticiSoftware.Translator.CLR.EnumRepTemplate";
							break;
						case "Delegate":
							typeType = "RusticiSoftware.Translator.CLR.DelegateRepTemplate";
							break;
						default:
							typeType = "UnknownType";
							break;
						}
						found = true;
					}
				}
				s.Seek (0, SeekOrigin.Begin);
				ret = (TypeRepTemplate)Deserialize (s, System.Type.GetType (typeType));
			} catch (Exception e) {
				Console.WriteLine ("WARNING -- (Deserialize) " + e.Message);
			}
			
			return ret;
		}


		public static TypeRepTemplate newInstance (Stream s)
		{
			return (TypeRepTemplate)Deserialize (s);
		}

		// Useful because it builds either an empty ClassRep or InterfaceRep or ...
		public abstract TypeRep mkEmptyRep ();
		
		#region Equality
		public bool Equals (TypeRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Uses != other.Uses) {
				if (Uses == null || other.Uses == null || Uses.Length != other.Uses.Length)
					return false;
				for (int i = 0; i < Uses.Length; i++) {
					if (Uses[i] != other.Uses[i])
						return false;
				}
			}
			
			return TypeName == other.TypeName && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			TypeRepTemplate temp = obj as TypeRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (TypeRepTemplate a1, TypeRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (TypeRepTemplate a1, TypeRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = base.GetHashCode ();
			if (Uses != null) {
				foreach (UseRepTemplate e in Uses) {
					hashCode ^= e.GetHashCode();
				}
			}
			return (Java ?? String.Empty).GetHashCode () ^ hashCode;
		}
		#endregion		
		
	}

	[XmlType("Enum")]
	public class EnumRepTemplate : TypeRepTemplate, IEquatable<EnumRepTemplate>
	{
		private List<EnumMemberRepTemplate> _members = null;
		[XmlArrayItem("Member")]
		public List<EnumMemberRepTemplate> Members {
			get {
				if (_members == null)
					_members = new List<EnumMemberRepTemplate> ();
				return _members;
			}
		}

		public EnumRepTemplate () : base()
		{
		}

		public EnumRepTemplate (List<EnumMemberRepTemplate> ms) : base()
		{
			_members = ms;
		}

		public override TypeRep mkEmptyRep ()
		{
			return new EnumRep ();
		}

		#region Equality
		public bool Equals (EnumRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Members != other.Members) {
				if (Members == null || other.Members == null || Members.Count != other.Members.Count)
					return false;
				for (int i = 0; i < Members.Count; i++) {
					if (Members[i] != other.Members[i])
						return false;
				}
			}
			
			return base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			EnumRepTemplate temp = obj as EnumRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (EnumRepTemplate a1, EnumRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (EnumRepTemplate a1, EnumRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = base.GetHashCode ();
			if (Members != null) {
				foreach (EnumMemberRepTemplate e in Members) {
					hashCode ^= e.GetHashCode();
				}
			}
			return hashCode;
		}
		#endregion		
	}

	[XmlType("Delegate")]
	public class DelegateRepTemplate : TypeRepTemplate, IEquatable<DelegateRepTemplate>
	{
		private List<ParamRepTemplate> _params = null;
		[XmlArrayItem("Param")]
		public List<ParamRepTemplate> Params {
			get {
				if (_params == null)
					_params = new List<ParamRepTemplate> ();
				return _params;
			}
		}

		public string Return {get; set;}
		
		public DelegateRepTemplate () : base()
		{
		}

		public DelegateRepTemplate (string retType, List<ParamRepTemplate> args) : base()
		{
			Return = retType;
			_params = args;
		}

		public override TypeRep mkEmptyRep ()
		{
			return new DelegateRep ();
		}

		#region Equality
		public bool Equals (DelegateRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Params != other.Params) {
				if (Params == null || other.Params == null || Params.Count != other.Params.Count)
					return false;
				for (int i = 0; i < Params.Count; i++) {
					if (Params[i] != other.Params[i])
						return false;
				}
			}
			
			return Return == other.Return && base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			DelegateRepTemplate temp = obj as DelegateRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (DelegateRepTemplate a1, DelegateRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (DelegateRepTemplate a1, DelegateRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = base.GetHashCode ();
			if (Params != null) {
				foreach (ParamRepTemplate e in Params) {
					hashCode ^= e.GetHashCode();
				}
			}
			return (Return ?? String.Empty).GetHashCode() ^ hashCode;
		}
		#endregion	
	}

	// Base Template for classes, interfaces, etc.
	[XmlType("Interface")]
	public class InterfaceRepTemplate : TypeRepTemplate, IEquatable<InterfaceRepTemplate>
	{


		[XmlArrayItem("Type")]
		public string[] Inherits { get; set; }

		private List<MethodRepTemplate> _methods = null;
		[XmlArrayItem("Method")]
		public List<MethodRepTemplate> Methods {
			get {
				if (_methods == null)
					_methods = new List<MethodRepTemplate> ();
				return _methods;
			}
		}
		
		private List<PropRepTemplate> _properties = null;
		[XmlArrayItem("Property")]
		public List<PropRepTemplate> Properties {
			get {
				if (_properties == null)
					_properties = new List<PropRepTemplate> ();
				return _properties;
			}
		}
		
		private List<FieldRepTemplate> _events = null;
		[XmlArrayItem("Event")]
		public List<FieldRepTemplate> Events {
			get {
				if (_events == null)
					_events = new List<FieldRepTemplate> ();
				return _events;
			}
		}
		
		private List<MethodRepTemplate> _indexers = null;
		[XmlArrayItem("Indexer")]
		public List<MethodRepTemplate> Indexers {
			get {
				if (_indexers == null)
					_indexers = new List<MethodRepTemplate> ();
				return _indexers;
			}
		}
		
		public InterfaceRepTemplate () : base()
		{
			Inherits = null;
		}

		public InterfaceRepTemplate (string typeName) : base(typeName)
		{
		}

		protected InterfaceRepTemplate (string tName, UseRepTemplate[] usePath, string[] inherits, List<MethodRepTemplate> ms, List<PropRepTemplate> ps, List<FieldRepTemplate> es, List<MethodRepTemplate> ixs, string[] imps, string javaTemplate) : base(tName, usePath, imps, javaTemplate)
		{
			Inherits = inherits;
			_methods = ms;
			_properties = ps;
			_events = es;
			_indexers = ixs;
		}

		public override TypeRep mkEmptyRep ()
		{
			return new InterfaceRep ();
		}
		
		#region Equality
		public bool Equals (InterfaceRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Inherits != other.Inherits) {
				if (Inherits == null || other.Inherits == null || Inherits.Length != other.Inherits.Length)
					return false;
				for (int i = 0; i < Inherits.Length; i++) {
					if (Inherits[i] != other.Inherits[i])
						return false;
				}
			}
			
			if (Methods != other.Methods) {
				if (Methods == null || other.Methods == null || Methods.Count != other.Methods.Count)
					return false;
				for (int i = 0; i < Methods.Count; i++) {
					if (Methods[i] != other.Methods[i])
						return false;
				}
			}

			if (Properties != other.Properties) {
				if (Properties == null || other.Properties == null || Properties.Count != other.Properties.Count)
					return false;
				for (int i = 0; i < Properties.Count; i++) {
					if (Properties[i] != other.Properties[i])
						return false;
				}
			}

			if (Events != other.Events) {
				if (Events == null || other.Events == null || Events.Count != other.Events.Count)
					return false;
				for (int i = 0; i < Events.Count; i++) {
					if (Events[i] != other.Events[i])
						return false;
				}
			}

			if (Indexers != other.Indexers) {
				if (Indexers == null || other.Indexers == null || Indexers.Count != other.Indexers.Count)
					return false;
				for (int i = 0; i < Indexers.Count; i++) {
					if (Indexers[i] != other.Indexers[i])
						return false;
				}
			}
			
			return base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			InterfaceRepTemplate temp = obj as InterfaceRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (InterfaceRepTemplate a1, InterfaceRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (InterfaceRepTemplate a1, InterfaceRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = base.GetHashCode ();
			if (Inherits != null) {
				foreach (string e in Inherits) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Methods != null) {
				foreach (MethodRepTemplate e in Methods) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Properties != null) {
				foreach (PropRepTemplate e in Properties) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Events != null) {
				foreach (FieldRepTemplate e in Events) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Indexers != null) {
				foreach (MethodRepTemplate e in Indexers) {
					hashCode ^= e.GetHashCode();
				}
			}
			return hashCode;
		}
		#endregion	
		
	}

	[XmlType("Class")]
	public class ClassRepTemplate : InterfaceRepTemplate, IEquatable<ClassRepTemplate>
	{

		private List<ConstructorRepTemplate> _constructors = null;
		[XmlArrayItem("Constructor")]
		public List<ConstructorRepTemplate> Constructors {
			get {
				if (_constructors == null)
					_constructors = new List<ConstructorRepTemplate> ();
				return _constructors;
			}
		}

		private List<FieldRepTemplate> _fields = null;
		[XmlArrayItem("Field")]
		public List<FieldRepTemplate> Fields {
			get {
				if (_fields == null)
					_fields = new List<FieldRepTemplate> ();
				return _fields;
			}
		}

		// Cast here? or in InterfaceRepT?
		private List<CastRepTemplate> _casts = null;
		[XmlArrayItem("Cast")]
		public List<CastRepTemplate> Casts {
			get {
				if (_casts == null)
					_casts = new List<CastRepTemplate> ();
				return _casts;
			}
		}

		public ClassRepTemplate ()
		{
		}

		public ClassRepTemplate (string typeName) : base(typeName)
		{
		}

		public ClassRepTemplate (string tName, UseRepTemplate[] usePath, string[] inherits, List<ConstructorRepTemplate> cs, List<MethodRepTemplate> ms, List<PropRepTemplate> ps, List<FieldRepTemplate> fs, List<FieldRepTemplate> es, List<MethodRepTemplate> ixs, List<CastRepTemplate> cts,
		string[] imports, string javaTemplate) : base(tName, usePath, inherits, ms, ps, es, ixs, imports, javaTemplate)
		{
			_constructors = cs;
			_fields = fs;
			_casts = cts;
		}

		public ClassRepTemplate (string tName, UseRepTemplate[] usePath, string[] inherits, List<ConstructorRepTemplate> cs, List<MethodRepTemplate> ms, List<PropRepTemplate> ps, List<FieldRepTemplate> fs, List<FieldRepTemplate> es, List<MethodRepTemplate> ixs, List<CastRepTemplate> cts) : base(tName, usePath, inherits, ms, ps, es, ixs, null, null)
		{
			_constructors = cs;
			_fields = fs;
			_casts = cts;
		}

		public override TypeRep mkEmptyRep ()
		{
			return new ClassRep ();
		}

		#region Equality
		public bool Equals (ClassRepTemplate other)
		{
			if (other == null)
				return false;
			
			if (Constructors != other.Constructors) {
				if (Constructors == null || other.Constructors == null || Constructors.Count != other.Constructors.Count)
					return false;
				for (int i = 0; i < Constructors.Count; i++) {
					if (Constructors[i] != other.Constructors[i])
						return false;
				}
			}

			if (Fields != other.Fields) {
				if (Fields == null || other.Fields == null || Fields.Count != other.Fields.Count)
					return false;
				for (int i = 0; i < Fields.Count; i++) {
					if (Fields[i] != other.Fields[i])
						return false;
				}
			}

			if (Casts != other.Casts) {
				if (Casts == null || other.Casts == null || Casts.Count != other.Casts.Count)
					return false;
				for (int i = 0; i < Casts.Count; i++) {
					if (Casts[i] != other.Casts[i])
						return false;
				}
			}

			return base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			ClassRepTemplate temp = obj as ClassRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (ClassRepTemplate a1, ClassRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (ClassRepTemplate a1, ClassRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			int hashCode = base.GetHashCode ();
			if (Constructors != null) {
				foreach (ConstructorRepTemplate e in Constructors) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Fields != null) {
				foreach (FieldRepTemplate e in Fields) {
					hashCode ^= e.GetHashCode();
				}
			}
			if (Casts != null) {
				foreach (CastRepTemplate e in Casts) {
					hashCode ^= e.GetHashCode();
				}
			}

			return hashCode;
		}
		#endregion	

		
	}




	[XmlType("Struct")]
	public class StructRepTemplate : ClassRepTemplate, IEquatable<StructRepTemplate>
	{

		public StructRepTemplate ()
		{
		}

		public StructRepTemplate (string typeName) : base(typeName)
		{
		}

		public StructRepTemplate (string tName, UseRepTemplate[] usePath, string[] inherits, List<ConstructorRepTemplate> cs, List<MethodRepTemplate> ms, List<PropRepTemplate> ps, List<FieldRepTemplate> fs, List<FieldRepTemplate> es, List<MethodRepTemplate> ixs, List<CastRepTemplate> cts,
		string[] imports, string javaTemplate) : base(tName, usePath, inherits, cs, ms, ps, fs, es, ixs, cts,
		imports, javaTemplate)
		{
		}

		public StructRepTemplate (string tName, UseRepTemplate[] usePath, string[] inherits, List<ConstructorRepTemplate> cs, List<MethodRepTemplate> ms, List<PropRepTemplate> ps, List<FieldRepTemplate> fs, List<FieldRepTemplate> es, List<MethodRepTemplate> ixs, List<CastRepTemplate> cts) : base(tName, usePath, inherits, cs, ms, ps, fs, es, ixs, cts,
		null, null)
		{
		}

		public override TypeRep mkEmptyRep ()
		{
			return new StructRep ();
		}
		
		#region Equality
		public bool Equals (StructRepTemplate other)
		{
			return base.Equals(other);
		}

		public override bool Equals (object obj)
		{
			
			StructRepTemplate temp = obj as StructRepTemplate;
			
			if (!Object.ReferenceEquals (temp, null))
				return this.Equals (temp);
			return false;
		}

		public static bool operator == (StructRepTemplate a1, StructRepTemplate a2)
		{
			return Object.Equals (a1, a2);
		}

		public static bool operator != (StructRepTemplate a1, StructRepTemplate a2)
		{
			return !(a1 == a2);
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
		#endregion
		
		
	}
}
