Ext.define('App.view.tpm.btl.HistoricalBTLDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbtldetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBTL', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('_Operation')
        }, {
            xtype: 'numberfield',
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('Number'),
            minValue: 0,
        }, {
            xtype: 'numberfield',
            name: 'PlanBTLTotal',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('PlanBTLTotal'),
            minValue: 0,
        }, {
            xtype: 'numberfield',
            name: 'ActualBTLTotal',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('ActualBTLTotal'),
            minValue: 0,
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('EndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name'),
            name: 'EventId',
            selectorWidget: 'event',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.event.Event',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.event.Event',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'EventName'
            }]
        }, {
            xtype: 'textfield',
            name: 'InvoiceNumber',
            regex: /^[0-9]*[0-9]$/,
            fieldLabel: l10n.ns('tpm', 'HistoricalBTL').value('InvoiceNumber'),
            regexText: l10n.ns('tpm', 'HistoricalBTL').value('InvoiceNumberRegex'),
        }]
    }
});
