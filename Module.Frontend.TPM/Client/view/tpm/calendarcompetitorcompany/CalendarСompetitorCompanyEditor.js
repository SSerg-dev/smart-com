Ext.define('App.view.tpm.calendarcompetitorcompany.CalendarCompetitorCompanyEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.calendarcompetitorcompanyeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    listeners: {
        show: function (window) {
            // Workaround для решения бага с прокрукой, если она должная появиться непосредственно при открытии окна
            window.doLayout();

            // скрываем кнопку Edit, если запись открыта из узла дерева
            if (Ext.ComponentQuery.query('producttree').length > 0) {
                window.down('#edit').setVisible(false);
            }
        }
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'CompanyName',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorCompany').value('CompanyName'),
        }]
    }
});  