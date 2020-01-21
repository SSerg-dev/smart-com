Ext.define('App.view.tpm.nonenego.NoneNegoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.nonenegoeditor',

    cls: 'readOnlyFields',
    width: 800,
    minWidth: 800,
	maxHeight: 500,
	storeLoaded: false,

    items: {
        xtype: 'editorform',
        columnsCount: 2,

        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            onTrigger2Click: function () {
                var fromDate = this.up('nonenegoeditor').down('[name=FromDate]');
                var toDate = this.up('nonenegoeditor').down('[name=ToDate]');
                var clientTreeObjectId = this.up('nonenegoeditor').down('[name=ClientTreeObjectId]');

                this.clearValue();
                this.setValue(null);
                clientTreeObjectId.setValue(0);
                fromDate.setReadOnly(true);
                toDate.setReadOnly(true);

                fromDate.addCls('field-for-read-only');
                toDate.addCls('field-for-read-only');
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }, {
                from: 'ObjectId',
                to: 'ClientTreeObjectId'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId')
        }, {
            xtype: 'treesearchfield',
            name: 'ProductTreeId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductTreeFullPathName'),
            selectorWidget: 'producttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.producttree.ProductTree',
                autoLoad: false,
                root: {}
            },
            onTrigger2Click: function () {
                var fromDate = this.up('nonenegoeditor').down('[name=FromDate]');
                var toDate = this.up('nonenegoeditor').down('[name=ToDate]');
                var productTreeObjectId = this.up('nonenegoeditor').down('[name=ProductTreeObjectId]');

                this.clearValue();
                productTreeObjectId.setValue(0);
                fromDate.setReadOnly(true);
                toDate.setReadOnly(true);

                fromDate.addCls('field-for-read-only');
                toDate.addCls('field-for-read-only');
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ProductTreeFullPathName'
            }, {
                from: 'ObjectId',
                to: 'ProductTreeObjectId'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductTreeObjectId')
        }, {
                xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
            name: 'MechanicId',
            selectorWidget: 'mechanic',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Mechanic',
            store: {
                type: 'simplestore',
                autoLoad: true,
                model: 'App.model.tpm.mechanic.Mechanic',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanic.Mechanic',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            onTrigger3Click: function () {
                var mechanicType = this.up('nonenegoeditor').down('[name=MechanicTypeId]');
                var discount = this.up('nonenegoeditor').down('[name=Discount]');

                this.clearValue();
                mechanicType.setReadOnly(true);
                discount.setValue(null);
                discount.setReadOnly(true);

                discount.addCls('field-for-read-only');
                mechanicType.addCls('field-for-read-only');

            }, 
            mapping: [{
                from: 'Name',
                to: 'MechanicName'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicTypeName'),
            name: 'MechanicTypeId',
            selectorWidget: 'mechanictype',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'MechanicType',
            allowBlank: false,
            allowOnlyWhitespace: false,
			readOnly: true,
            store: {
                type: 'simplestore',
                autoLoad: true,
                model: 'App.model.tpm.mechanictype.MechanicType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanictype.MechanicType',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            onTrigger3Click: function () {
                var discount = this.up().down('[name=Discount]');

                this.clearValue();
                discount.setValue(null);
                discount.addCls('field-for-read-only');
            }, 
            mapping: [{
                from: 'Name',
                to: 'MechanicTypeName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('Discount'),
            minValue: 1,
            maxValue: 100,
            readOnly: true,
            allowBlank: false,
            allowDecimals: false,
        }, {
            xtype: 'datefield',
            name: 'FromDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate'),
            allowBlank: false,
            //minValue: new Date(),
            isCurrentFieldValid: true,
            editable: false,
            format: 'd.m.Y',
			trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
			firstChange: true,
			valueChanged: false,
            onTrigger2Click: function () {
                this.reset();
            },
            validator: function () {
				if (!this.isCurrentFieldValid) {
						return l10n.ns('tpm', 'NoneNego').value('ValidatePeriodError');
				}
                return true;
            },
            listeners: {
                afterrender: function (field) {
                    var minValue = new Date();
                    var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                    var minValueInt = minValue.getTime();
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                   
                    field.getPicker().setValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                    if (currentRole !== 'SupportAdministrator') {

                        field.getPicker().setMinDate(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                    }
                },
                change: function (newValue, oldValue) {
                    var toDate = this.up('form').down('[name=ToDate]');
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                    if (currentRole !== 'SupportAdministrator') {

                        toDate.setMinValue(newValue.getValue());
                    }
                    }
            }
        }, {
            xtype: 'datefield',
            name: 'ToDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            minValue: new Date(),
            isCurrentFieldValid: true,
            editable: false,
            format: 'd.m.Y',
            trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
            onTrigger2Click: function () {
                this.reset();
            },
            validator: function () {
                if (!this.isCurrentFieldValid) {
                    return l10n.ns('tpm', 'NoneNego').value('ValidatePeriodError');
                }

                return true;
            },
                listeners: {
                    afterrender: function (button) {

                        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                        if (currentRole === 'SupportAdministrator') {
                            button.minValue = null;
                        } 
                    },
                change: function (newValue, oldValue) {
                    var fromDate = this.up('form').down('[name=FromDate]');
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                 //   if (currentRole !== 'SupportAdministrator') {

                        fromDate.setMaxValue(newValue.getValue());
                   // }
                }
            }
        }, {
            xtype: 'datefield',
            name: 'CreateDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate'),
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