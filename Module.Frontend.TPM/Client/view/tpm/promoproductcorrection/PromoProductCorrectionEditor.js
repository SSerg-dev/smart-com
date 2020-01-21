Ext.define('App.view.tpm.promoproductcorrection.PromoProductCorrectionEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoproductcorrectioneditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    afterWindowShow: function (scope, isCreating) {
        scope.down('searchfield[name=Number]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [
            {
                xtype: 'searchfield',
                listeners: {
                    change: function (button) {
                        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                        var ZREP = promoproductcorrectioneditor.down('[name=ZREP]');

                        ZREP.clearValue();
                        if (button.value) {
                            ZREP.setReadOnly(false);
                            ZREP.removeCls('field-for-read-only');
                             
                        } else {
                            ZREP.setReadOnly(true);
                            ZREP.addCls('field-for-read-only');
                        } 
                    },
                    beforeRender: function (button) {
                        if (button.value ) {
                            button.setReadOnly(true);
                        } else {
                            button.setReadOnly(false);
                        } 
                    },
                    focus: function (field) {
                        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                      
                        var ZREP = promoproductcorrectioneditor.down('[name=ZREP]');
                          
                            if (!field.isCreate) {
                                field.setReadOnly(true);
                                field.addCls('field-for-read-only');
                                ZREP.setReadOnly(true);
                                ZREP.addCls('field-for-read-only');
                             
                            }
                      
                      
                        
                        if (ZREP.readOnly  ) {
                            ZREP.addCls('field-for-read-only');
                        }


                             
                    }
                },
               
                onTrigger1Click: function () {
                 
                    //Код из за переопределения клика на лупу
                    var picker = this.createPicker(),
                        grid, columns;

                    if (picker) {
                        picker.show();
                        if (this.multiSelect) {
                            grid = this.getGrid();
                            columns = grid.headerCt.getGridColumns();
                            if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd')) {
                                columns[0].show();
                            }
                        }
                        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                        if (currentRole !== 'SupportAdministrator') {

                            var status = ['DraftPublished', 'OnApproval', 'Approved', 'Planned'];
                            this.getStore().setFixedFilter('PromoStatusNameFilter', {
                                property: 'PromoStatus.SystemName',
                                operation: 'In',
                                value: status
                            });
                        }
                        this.getStore().load();

                    }
                    var choosepromo = Ext.ComponentQuery.query('choosepromo')[0];

                    choosepromo.down('#datatable').multiSelect = false;

                    //var store = grid.getStore();
                    //
                    //store.setFixedFilter('PromoId', {
                    //    property: 'PromoId',
                    //    operation: 'Equals',
                    //    value: promogrid.promoId
                    //});PromoStatusName

                },
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('Number'), 
                isCreate: false,
                readOnly: true,
                name: 'Number',
                selectorWidget: 'choosepromo',
                valueField: 'Id',
                displayField: 'Number',
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.promo.Promo',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.promo.Promo',
                            modelId: 'efselectionmodel'
                        }]
                    }
                },
                mapping: [{
                    from: 'Number',
                    to: 'Number'
                }]
            },
            {
                xtype: 'searchfield',
                 
                onTrigger1Click: function () {
                    //нажатие на лупу, должен быть фильтр по продуктам
                    var me = this;
                    var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                    var id = promoproductcorrectioneditor.down('[name=Number]');

                    if (id.value) {
                        me.store.getProxy().extraParams = [];
                        me.store.getProxy().extraParams.promoId = breeze.DataType.Guid.fmtOData(id.value);

                        var picker = this.createPicker();
                        if (picker) {
                            picker.show();
                            me.store.load();
                        }
                    }
                },
                readOnly: true,
                editable: false,
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ZREP'),
                name: 'ZREP',
                selectorWidget: 'product',
                valueField: 'Id',
                displayField: 'ZREP',
                listeners: {
                    change: function (button) {
                         
                        //вызываем метод заполняющий скрытое поле промопродукт

                        var me = App.app.getController('tpm.promoproductcorrection.PromoProductCorrection');
                        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                        var promoId = promoproductcorrectioneditor.down('[name=Number]');
                        var productId = promoproductcorrectioneditor.down('[name=ZREP]');
                        me.saveModel(promoId.getValue(), productId.getValue());
                    },
                   

                },
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
                    from: 'ZREP',
                    to: 'ZREP'
                }]
            }, 
            {
                xtype: 'textfield',
                name: 'PromoProductId',
                fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
                hidden: true

            },
           
            {
                xtype: 'numberfield',
                name: 'PlanProductUpliftPercentCorrected',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductUpliftPercentCorrected'), 
                validator: function (value) {
                    if (value <= 0) {
                        return l10n.ns('tpm', 'PromoProductCorrection').value('GreaterThanZero');
                    } else {
                        return true;
                    }
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'CreateDate',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('CreateDate'),
                //allowBlank: false,
                //readOnly: true,
                //editable: false,
                //format: 'd.m.Y',

                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                //listeners: {
                //    writeablechange: function (field) {
                //        if (field.readOnly == false) {
                //            field.setReadOnly(true);
                //        }
                //    }
                //}
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'ChangeDate',
                
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('ChangeDate'),
                allowBlank: false,
               // readOnly: true,
               // editable: false,
               // listeners: {
               //          
               //     writeablechange: function (field) {
               //         if (field.readOnly == false) {
               //             field.setReadOnly(true);
               //         }
               //     }
               // },
                //format: 'd.m.Y',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
                 
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'UserName',
                fieldLabel: l10n.ns('tpm', 'PromoProductCorrection').value('UserName'), 
            },]
    }
});
