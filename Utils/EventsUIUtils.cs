using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class EventsUIUtils
    {
        public static string GetEvaluateBuyPrice(int discount)
        {
            Game.game.caravan.GetEvaluatePerkSumMax(out var leaderPerksSum, out var leaderPerksMax);

            var assessment = "";
            if (leaderPerksSum < 4 && leaderPerksMax < 2)
                assessment = "Not experienced enough to judge these prices".FromDictionary();
            else if (leaderPerksSum >= 6 || leaderPerksMax >= 3)
            {
                if (discount > 100)
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), "<color=red>" + discount + "</color>");
                }
                else if (discount < 100)
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), "<color=green>" + discount + "</color>");
                }
                else
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), discount);
                }
            }
            else if (discount >= 150)
            {
                assessment = "Significantly higher than the local price".FromDictionary();
            }
            else if (discount >= 110 && discount < 150)
            {
                assessment = "Higher than the local price".FromDictionary();
            }
            else if (discount >= 90 && discount < 110)
            {
                assessment = "Close to the local price".FromDictionary();
            }
            else if (discount >= 50 && discount < 90)
            {
                assessment = "Lower than the local price".FromDictionary();
            }
            else if (discount < 50)
            {
                assessment = "Significantly lower than the local price".FromDictionary();
            }

            return assessment;
        }

        public static string GetEvaluateSellPrice(int discount)
        {
            Game.game.caravan.GetEvaluatePerkSumMax(out var leaderPerksSum, out var leaderPerksMax);

            var assessment = "";
            if (leaderPerksSum < 4 && leaderPerksMax < 2)
                assessment = "Not experienced enough to judge these prices".FromDictionary();
            else if (leaderPerksSum >= 6 || leaderPerksMax >= 3)
            {
                if (discount < 100)
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), "<color=red>" + discount + "</color>");
                }
                else if (discount > 100)
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), "<color=green>" + discount + "</color>");
                }
                else
                {
                    assessment = string.Format("{0}% of the local price".FromDictionary(), discount);
                }
            }
            else if (discount >= 150)
            {
                assessment = "Significantly higher than the local price".FromDictionary();
            }
            else if (discount >= 110 && discount < 150)
            {
                assessment = "Higher than the local price".FromDictionary();
            }
            else if (discount >= 90 && discount < 110)
            {
                assessment = "Close to the local price".FromDictionary();
            }
            else if (discount >= 50 && discount < 90)
            {
                assessment = "Lower than the local price".FromDictionary();
            }
            else if (discount < 50)
            {
                assessment = "Significantly lower than the local price".FromDictionary();
            }

            return assessment;
        }

    }
}
