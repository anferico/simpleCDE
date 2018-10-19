using System.Text;

namespace Progetto {
	public class YamlTransformer {
		private string yamlString;

		public YamlTransformer(string yamlString) {
			this.yamlString = yamlString;
		}

		public string Transform() {
			if (string.IsNullOrEmpty(yamlString))
				return yamlString;
			var outputString = new StringBuilder();
			int p = 0, lastIndentationLevel = 0, currentIndentationLevel = 0;
			while (p < yamlString.Length) {
				if (yamlString[p] != '\n')
					outputString.Append(yamlString[p++]);
				else {
					currentIndentationLevel = 0;
					while (++p < yamlString.Length && yamlString[p] == '\t')
						currentIndentationLevel++;
					if (currentIndentationLevel > lastIndentationLevel)
						outputString.Append('{', currentIndentationLevel - lastIndentationLevel);
					else if (currentIndentationLevel < lastIndentationLevel) {
						outputString.Append('}', lastIndentationLevel - currentIndentationLevel);
						if (lastIndentationLevel - currentIndentationLevel == currentIndentationLevel)
							outputString.Append("}{");
					} else outputString.Append("}{");
					lastIndentationLevel = currentIndentationLevel;
				}
			}
			return outputString.ToString();
		}
	}
}