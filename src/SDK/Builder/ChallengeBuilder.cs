using System;
using System.Collections.Generic;

namespace Silanis.ESL.SDK.Builder
{
	public class ChallengeBuilder : AuthenticationBuilder
	{
		private string question;
		private IList<Challenge> challenges = new List<Challenge>();

		private ChallengeBuilder (string question)
		{
			this.question = question;
		}

		public static ChallengeBuilder FirstQuestion(string question)
		{
			return new ChallengeBuilder (question);
		}

        public ChallengeBuilder Answer(string answer)
		{
            challenges.Add(new Challenge(question, answer, Challenge.MaskOptions.None));
            return this;
        }

        [Obsolete]
		public ChallengeBuilder Answer(string answer, Challenge.MaskOptions maskOption)
		{
			challenges.Add (new Challenge(question, answer, maskOption));
			return this;
		}

        public ChallengeBuilder AnswerWithMaskInput(string answer)
        {
            challenges.Add (new Challenge(question, answer, Challenge.MaskOptions.MaskInput));
            return this;
        }

		public ChallengeBuilder SecondQuestion (string question)
		{
			this.question = question;
			return this;
		}

		public override Authentication Build()
		{
			if (QuestionProvided () && challenges.Count == 0)
			{
				throw new EslException ("Question challenge was provided with no answer",null);
			}
			Authentication result = new Authentication(challenges);

			return result;
		}

		private bool QuestionProvided ()
		{
			return question != null && question.Trim ().Length > 0;			
		}
	}
}