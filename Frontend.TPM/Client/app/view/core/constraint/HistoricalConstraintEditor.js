Ext.define('App.view.core.constraint.HistoricalConstraintEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalconstrainteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalConstraint', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Prefix',
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('Prefix'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Value',
            fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('Value'),
        }]
    }
});
