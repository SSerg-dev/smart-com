Ext.define('App.view.tpm.filter.SelectOperationWindow', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.selectoperationwindow',
    title: 'Add panel',

    width: 230,
    height: 150,
    minWidth: 230,
    minHeight: 150,
    maxWidth: 230,
    maxHeight: 150,

    items: [{
        xtype: 'radiogroup',
        cls: 'and-or-radiogroup',
        columns: 2,
        vertical: true,
        //padding: '10 15 20 25',
        //margin: '0 0 0 15',
        items: [
            { boxLabel: 'AND', name: 'rb', inputValue: 1, checked: true },
            { boxLabel: 'OR', name: 'rb', inputValue: 2 },
        ]        
    }],
    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: 'Add panel',//l10n.ns('core', 'buttons').value('ok'),
        itemId: 'addpanel',
    }],

    listeners: {
        show: function (wind, eOpts) {
            var radios = wind.down('radiogroup').items.items;

            Ext.each(radios, function (r) {
                r.fieldLabelTip.setDisabled(true);
                r.fieldValueTip.setDisabled(true);
            });     
        },
    }
})