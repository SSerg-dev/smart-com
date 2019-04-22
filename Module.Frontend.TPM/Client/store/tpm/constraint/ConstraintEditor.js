Ext.override(App.view.core.constraint.ConstraintEditor, {
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'combobox',
            name: 'Prefix',
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'Constraint').value('Prefix'),
            editable: false,
            queryMode: 'local',
            valueField: 'id',
            store: {
                type: 'constraintprefixstore'
            },
            listeners: {
                afterrender: function () {
                    this.select(this.store.getAt(0));
                }
            }
        }, {
            xtype: 'treesearchfield',
            name: 'Value',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'ObjectId',
            displayField: 'ObjectId',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            onTrigger2Click: function () {
                var clientTreeObjectId = this.up('editorform').down('[name=Value]');
                this.clearValue();
                this.setValue(null);
                clientTreeObjectId.setValue(0);
            },
            mapping: [{
                from: 'ObjectId',
                to: 'Value'
            }]
        },
            //{
            //xtype: 'singlelinedisplayfield',
            //name: 'Value',
            //allowBlank: false,
            //fieldLabel: l10n.ns('core', 'Constraint').value('Value')
            //}
        ]
    }
});