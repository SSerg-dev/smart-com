Ext.define('App.view.tpm.schedule.SelectBudgetYearWindow', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.selectbudgetyearwindow',
    title: 'Select budget year',

    width: 230,
    height: 130,
    minWidth: 230,
    minHeight: 130,
    maxWidth: 230,
    maxHeight: 130,

    items: [{
        xtype: 'combobox',
        name: 'PromoBudgetYear',
        id: 'budgetYearCombo',
        valueField: 'year',
        editable: false,
        displayField: 'year',
        style: 'margin:10px',
        queryMode: 'local',
        store: Ext.create('Ext.data.Store', {
            fields: ['year']
        }),
        listConfig: {
            cls: 'always-on-top'
        },
    }],
    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'okBudgetYearButton',
        ui: 'green-button-footer-toolbar',
    }]
})