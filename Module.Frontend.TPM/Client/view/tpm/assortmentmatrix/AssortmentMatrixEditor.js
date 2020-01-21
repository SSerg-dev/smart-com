Ext.define('App.view.tpm.assortmentmatrix.AssortmentMatrixEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.assortmentmatrixeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            onTrigger2Click: function () {
                var clientTreeObjectId = this.up('assortmentmatrixeditor').down('[name=ClientTreeObjectId]');

                this.clearValue();
                this.setValue(null);
                clientTreeObjectId.setValue(0);
            },
            mapping: [{
                from: 'Name',
                to: 'ClientTreeName'
            }, {
                from: 'ObjectId',
                to: 'ClientTreeObjectId'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId')
        }, {
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC'),
            selectorWidget: 'product',
            valueField: 'Id',
            displayField: 'EAN_PC',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.product.Product',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.product.Product',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'EAN_PC',
                to: 'ProductEAN_PC'
            }]
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('StartDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                afterrender: function (field) {
                    var minValue = new Date();
                    var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                    var minValueInt = minValue.getTime();
                    field.setMinValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                    field.getPicker().setValue(field.minValue);
                    if (App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
                        field.setMinValue(null);
                        field.validate();
                    }
                },
                change: function (field) {
                    var endDate = field.up('editorform').down('[name=EndDate]');

                    if (field.getValue()) {
                        var minValue = new Date(Date.parse(field.getValue()) + 24 * 60 * 60 * 1000);
                        endDate.setMinValue(minValue);
                    }
                }
            }
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EndDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                change: function (field) {
                    var startDate = field.up('editorform').down('[name=StartDate]');

                    if (field.getValue()) {
                        var maxValue = new Date(Date.parse(field.getValue()) - 24 * 60 * 60 * 1000);
                        startDate.setMaxValue(maxValue);
                    }
                },
            }
        }, {
            xtype: 'datefield',
            name: 'CreateDate',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('CreateDate'),
            allowBlank: false,
            readOnly: true,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                    }
                }
            }
        }]
    }
});