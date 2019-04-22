using Module.Persist.TPM.Model.TPM;
using System.Collections.Generic;

namespace Module.Persist.TPM.PromoStateControl
{
    public interface IPromoState
    {
        /// <summary>
        /// Возвращает название состояния
        /// </summary>
        string GetName();

        /// <summary>
        /// Возвращает список ролей для состояния
        /// </summary>
        List<string> GetRoles();

        /// <summary>
        /// Возвращает текущую модель
        /// </summary>
        Promo GetModel();

        /// <summary>
        /// Возвращает список доступных состояний и ролей
        /// </summary>
        Dictionary<string, List<string>> GetAvailableStates();

        /// <summary>
        /// Изменяет/обновляет состояние
        /// </summary>
        bool ChangeState(Promo promoModel, string userRole, out string massage);

        /// <summary>
        /// Изменяет/обновляет состояние
        /// </summary>
        bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string massage);
    }
}
