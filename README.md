ExtensionMethods - collection of usable C# collection methods
================


IEnumerableExtensions
---------------------
  * _ForEach_ :  allows you to execute something for each of the items without any transformations/data manipulation 

*BoolExtensions*
  * In<T> : Returns true if value is contained in given collection
  * Between<T> : Returns true if the value is between lower and upper. Works for everything that implement IComparable e.g. Int, Double etc.

*EnumExtensions*
  * GetDescription : Makes it easy to get the description (friendly name with spaces etc.) for a given value of the enum

*NullableDateTimeExtensions*
  * ToShortDateString : Returns short date string or empty string if the value is null

*NullableExtensions*
  * ToStringOrEmpty<T> : Returns value of nullable type as string ot empty string if the value is null


*UIExtensions*
  * FindChild : UI child finder, e.g.: comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox") as TextBox;
  * GetScrollViewer : Returns ScrollViewer of this ListBox or null

*StringExtensions*
  * IsDigitsOnly : Checks if the string consists of digits only
