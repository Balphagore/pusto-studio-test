using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class DigitInputValidator : TMP_InputValidator
{
	[SerializeField]
	private int _value;

	public override char Validate(ref string text, ref int pos, char ch)
	{
		if (int.TryParse(text + ch, out int num))
		{
			if (num >= 0 && num <= _value)
			{
				text = num.ToString("00");
				pos = text.Length;
				return ch;
			}
		}
		return '\0';
	}
}
