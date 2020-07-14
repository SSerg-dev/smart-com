Ext.define('App.view.tpm.mechanictype.MechanicTypeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.mechanictypeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'MechanicType').value('Name'),
        }, {
            xtype: 'numberfield',
            name: 'Discount',
            minValue: 0,
            maxValue: 100,
            allowDecimals: true, 
            allowExponential: false, 
            allowBlank: true, 
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'MechanicType').value('Discount')
        }, {
            xtype: 'baseclienttreesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'MechanicType').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            clientTreeIdValid: true,
            allowBlank: true,
            allowOnlyWhitespace: true,
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            //listeners: {
            //    beforerender: function (picker) {
            //        var newOnTrigger1Click = function () {
            //            var mechanicTypeController = App.app.getController('tpm.mechanictype.MechanicType');
            //            mechanicTypeController.onTrigger1Click(picker);
            //        }
            //        picker.onTrigger1Click = newOnTrigger1Click;
            //    }
            //},

            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }]
    }
});
