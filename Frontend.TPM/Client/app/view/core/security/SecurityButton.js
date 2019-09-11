// ----------------------------------------------------------------------
// <copyright file="SecurityButton.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.view.core.security.SecurityButton', {
    extend: 'Ext.button.Button',
    alias: 'widget.securitybutton',
    ui: 'security-button',
    textAlign: 'left',

    setText: function (text) {
        if (!Ext.isEmpty(text)) {
            arguments[0] = '<span class="securitybutton-text">' + text + '</span>';
        }
        this.callParent(arguments);
    }
});