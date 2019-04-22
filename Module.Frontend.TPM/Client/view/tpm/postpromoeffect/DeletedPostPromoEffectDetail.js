Ext.define('App.view.tpm.postpromoeffect.DeletedPostPromoEffectDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpostpromoeffectdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek1',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek2',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TotalEffect',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
        }]
    }
})
