using System;
using System.IO;
using System.Collections.Generic;
namespace Progetto {
	public class CDEYamlParser {
		private LexicalAnalyzer lexer;

		public Specification Parse(string inputFileName) {
			if (File.Exists(inputFileName)) {
				var transformer = new YamlTransformer(File.ReadAllText(inputFileName));
				lexer = new LexicalAnalyzer(transformer.Transform());
			} else Error("File does not exist.");
			return spec();
		}

		private Specification spec() {
			return new Specification(srv(), agn(), rel());
		}

		private List<Service> srv() {
			var services = new List<Service>();
			Match(Token.SERVICES, Token.COLON);
			while (lexer.Lookahead != Token.AGENTS) {
				Match(Token.INDENTINC);
				string serviceName = str();
				Match(Token.COLON, Token.INDENTINC, Token.UNITS, Token.COLON);
				ws();
				int serviceUnits = intg();
				Match(Token.INDENTDEC, Token.INDENTDEC);
				services.Add(new Service(serviceName, serviceUnits));
			}
			return services;
		}

		private List<Agent> agn() {
			var agents = new List<Agent>();
			Match(Token.AGENTS, Token.COLON);
			while (lexer.Lookahead != Token.RELATIONS && lexer.Lookahead != Token.ENDOFINPUT) {
				Match(Token.INDENTINC);
				string serviceImplemented = str();
				Match(Token.COLON);
				string implementingClass = cls();
				var events = evt();
				List<string> requirements = null, provisions = null;

				if (lexer.Lookahead != Token.INDENTDEC) {
					Match(Token.INDENTINC);
					if (lexer.Lookahead == Token.REQUIRES) {
						Match(Token.REQUIRES, Token.COLON);
						ws();
						requirements = strlist();
						Match(Token.INDENTDEC);
						if (lexer.Lookahead != Token.INDENTDEC) {
							Match(Token.INDENTINC, Token.PROVIDES, Token.COLON);
							ws();
							provisions = strlist();
							Match(Token.INDENTDEC);
						}
					} else if (lexer.Lookahead == Token.PROVIDES) {
						Match(Token.PROVIDES, Token.COLON);
						ws();
						provisions = strlist();
						Match(Token.INDENTDEC);
					} else Error("Unexpected token.");
				}
				Match(Token.INDENTDEC);
				agents.Add(new Agent(serviceImplemented, implementingClass, events, requirements, provisions));
			}
			return agents;
		}

		private List<Relation> rel() {
			List<Relation> relations = null;
			if (lexer.Lookahead != Token.ENDOFINPUT) {
				relations = new List<Relation>();
				Match(Token.RELATIONS, Token.COLON);
				while (lexer.Lookahead != Token.ENDOFINPUT) {
					Match(Token.INDENTINC, Token.DASH);
					ws();
					Match(Token.SQROPEN);
					string subService = str();
					Match(Token.COLON);
					ws();
					string subEvent = str();
					Match(Token.COMMA);
					ws();
					string leadService = str();
					Match(Token.COLON);
					ws();
					string leadEvent = str();
					Match(Token.SQRCLOSE, Token.INDENTDEC);
					relations.Add(new Relation(subService, subEvent, leadService, leadEvent));
				}
			}
			return relations;
		}

		private string cls() {
			Match(Token.INDENTINC, Token.CLASS, Token.COLON);
			ws();
			string className = str();
			Match(Token.INDENTDEC);
			return className;
		}

		private List<Event> evt() {
			var events = new List<Event>();
			Match(Token.INDENTINC, Token.EVENTS, Token.COLON, Token.INDENTINC);
			while (lexer.Lookahead != Token.INDENTDEC) {
				string eventName = str();
				Match(Token.COLON, Token.INDENTINC);
				List<string> preconditions = null;
				if (lexer.Lookahead == Token.AFTER) {
					Match(Token.AFTER, Token.COLON);
					ws();
					preconditions = strlist();
					Match(Token.INDENTDEC, Token.INDENTINC);
				}
				Match(Token.HANDLER, Token.COLON);
				ws();
				string handler = str();
				Match(Token.INDENTDEC);
				events.Add(new Event(eventName, preconditions, handler));
			}
			Match(Token.INDENTDEC, Token.INDENTDEC);
			return events;
		}

		private List<string> strlist() {
			var stringList = new List<string>();
			Match(Token.SQROPEN);
			stringList.Add(str());
			while (lexer.Lookahead != Token.SQRCLOSE) {
				Match(Token.COMMA);
				ws();
				stringList.Add(str());
			}
			Match(Token.SQRCLOSE);
			return stringList;
		}

		private void ws() {
			Match(Token.WHITESPACE);
		}

		private string str() {
			Match(Token.LITERAL);
			return lexer.TokenValue;
		}

		private int intg() {
			Match(Token.LITERAL);
			int result;
			if (!int.TryParse(lexer.TokenValue, out result))
				Error("Found a string while expecting an integer.");
			else if (result <= 0)
				Error("Non-positive integer.");
			return result;
		}

		private void Match(params Token[] tokens) {
			foreach (var token in tokens)
				if (lexer.GetNextToken() != token)
					Error("Unexpected token.");
		}

		private void Error(string message) {
			throw new Exception(message);
		}
	}
}