ExtensionMethods - collection of usable C# collection methods
================


*IEnumerableExtensions*
  * <b>ForEach</b> :  allows you to execute something for each of the items without any transformations/data manipulation 

*BoolExtensions*
  * <b>In</b> : Returns true if value is contained in given collection
  * <b>Between</b> : Returns true if the value is between lower and upper. Works for everything that implement IComparable e.g. Int, Double etc.

*EnumExtensions*
  * <b>GetDescription</b> : Makes it easy to get the description (friendly name with spaces etc.) for a given value of the enum

*NullableDateTimeExtensions*
  * <b>ToShortDateString</b> : Returns short date string or empty string if the value is null

*NullableExtensions*
  * <b>ToStringOrEmpty</b> : Returns value of nullable type as string ot empty string if the value is null


*UIExtensions*
  * <b>FindChild</b> : UI child finder, e.g.: comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox") as TextBox;
  * <b>GetScrollViewer</b> : Returns ScrollViewer of this ListBox or null

*StringExtensions*
  * <b>IsDigitsOnly</b> : Checks if the string consists of digits only
