Ext.define('App.view.tpm.btl.DeletedBTLDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedbtldetail',
    width: 500,
    minWidth: 500,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            labelWidth: '105px',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'numberfield',
            name: 'PlanBTLTotal',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('PlanBTLTotal'),
            minValue: 0,
        }, {
            xtype: 'numberfield',
            name: 'ActualBTLTotal',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('ActualBTLTotal'),
            minValue: 0,
        }, {
            xtype: 'container',
            cls: 'x-field x-table-plain x-form-item x-form-type-text x-form-dirty x-box-item x-field-detail-form-field x-vbox-form-item',
            flex: 1,
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            defaults: {
                labelAlign: 'left',
                flex: 1,
            },
            items: [{
                xtype: 'datefield',
                name: 'StartDate',
                fieldLabel: l10n.ns('tpm', 'BTL').value('StartDate'),
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datefield',
                name: 'EndDate',
                fieldLabel: l10n.ns('tpm', 'BTL').value('EndDate'),
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        },  {
            xtype: 'textfield',
            name: 'InvoiceNumber',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('InvoiceNumber'),
        },  {
            xtype: 'singlelinedisplayfield',
            name: 'EventName',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('EventName'),
        }]
    }
})