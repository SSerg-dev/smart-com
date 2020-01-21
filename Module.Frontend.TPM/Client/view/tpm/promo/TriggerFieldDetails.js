Ext.define('App.view.tpm.promo.TriggerFieldDetails', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.triggerfielddetails',
    trigger1Cls: 'form-info-trigger',
    cls: 'promo-activity-details-window-field',
    labelCls: 'borderedField-label',
    width: '100%',
    labelWidth: 190,
    labelSeparator: '',
    triggerNoEditCls: 'inputReadOnly',
    editable: false,
    regex: /^(-?)(0|([1-9][0-9]*))(\,[0-9]+)?$/i,
    regexText: l10n.ns('tpm', 'PromoActivity').value('triggerfieldOnlyNumbers'),
    maxLength: 100,
    windowType: 'promoactivitydetailsinfo',
    isReadable: true,
    crudAccess: [],

    listeners: {
        beforerender: function (me) {
            if (me.listenersToAdd) {
                me.on(me.listenersToAdd);
            }
        },
        afterrender: function (me) {
            me.setEditable(me.editable);
        }
    },

    valueToRaw: function (value) {
        return Ext.util.Format.number(value, '0.00');
    },

    rawToValue: function (value) {
        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
        return isNaN(parsedValue) ? null : parsedValue;
    },

    onTrigger1Click: function (el) {
        var me = this;
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);
        var showMessage = false;

        switch (me.windowType) {
            case 'promoactivitydetailsinfo':
                var newWindow = Ext.create('App.view.core.base.BaseModalWindow', {
                    title: 'Details',
                    width: '90%',
                    minWidth: '90%',
                    height: '90%',
                    minHeight: '90%',
                    items: [{
                        xtype: me.windowType,
                        promoId: record.data.Id
                    }],
                    buttons: [{
                        text: l10n.ns('core', 'buttons').value('cancel'),
                        itemId: 'close'
                    }]
                });

                me.dataIndexes.forEach(function (field) {
                    var columns = newWindow.down('directorygrid').columns;
                    columns.forEach(function (column) {
                        if (field === column.dataIndex) {
                            column.hidden = false;
                        }
                    });
                });

                var promoProductStore = newWindow.down('directorygrid').getStore();
                promoProductStore.setFixedFilter('PromoIdFilter', {
                    operator: 'and',
                    rules: [{
                        property: 'PromoId',
                        operation: 'Equals',
                        value: record.data.Id
                    }]
                });
                promoProductStore.load();

                break;

            case 'promoproductsview':
                var newWindow = Ext.create('App.view.core.base.BaseModalWindow', {
                    title: 'Details',
                    width: '90%',
                    minWidth: '90%',
                    height: '90%',
                    minHeight: '90%',
                    items: [{
                        xtype: me.windowType,
                        promoId: record.data.Id,
                        isReadable: this.isReadable,
                        defaultValue: this.defaultValue,
                        crudAccess: this.crudAccess,
                    }],
                    buttons: [{
                        text: l10n.ns('core', 'buttons').value('cancel'),
                        itemId: 'close'
                    }, {
                        text: l10n.ns('core', 'buttons').value('ok'),
                        itemId: 'ok',
                        ui: 'green-button-footer-toolbar',
                        listeners:
                        {
                            afterrender: function (button) {
                                if (me.isReadable || me.defaultValue == true) {
                                    button.setVisible(false);
                                }
                            },
                            click: function (button) {
                                var promoProductsView = button.up('basewindow').down('promoproductsview'),
                                    storePromoProductsView = promoProductsView.storePromoProductsView;

                                promoProductsView.up('basewindow').setLoading(l10n.ns('core').value('savingText'));

                                if (storePromoProductsView.data.length > 0) {
                                    storePromoProductsView.sync({
                                        success: function () {
                                            // закрывается окно
                                            promoProductsView.up('basewindow').setLoading(false);
                                            promoProductsView.up('basewindow').close();
                                        },
                                        failure: function () {
                                            promoProductsView.up('basewindow').setLoading(false);
                                        }
                                    });
                                } else {
                                    // закрывается окно
                                    promoProductsView.up('basewindow').setLoading(false);
                                    promoProductsView.up('basewindow').close();
                                }
                            }
                        }
                    }],
                });

                var promoproductsview = newWindow.down('promoproductsview');

                var promoId = promoproductsview.promoId;
                var store = promoproductsview.down('directorygrid').getStore();

                store.getProxy().extraParams.promoId = breeze.DataType.Guid.fmtOData(promoId);

                var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
                if (promoeditorcustom.tempEditUpliftId) {
                    store.getProxy().extraParams.tempEditUpliftId = promoeditorcustom.tempEditUpliftId;
                } else {
                    store.getProxy().extraParams.tempEditUpliftId = null;
                    promoeditorcustom.tempEditUpliftId = App.UserInfo.getUserName() + Date.now();
                }
                store.load();

                if (this.defaultValue == true && !this.isReadable) {
                    showMessage = true;
                } else { showMessage = false }

                break;

            default:
                var newWindow = Ext.create('App.view.core.base.BaseModalWindow', {
                    title: 'Details',
                    width: '90%',
                    minWidth: '90%',
                    height: '90%',
                    minHeight: '90%',
                    items: [{
                        xtype: me.windowType,
                        promoId: record.data.Id
                    }],
                    buttons: [{
                        text: l10n.ns('core', 'buttons').value('cancel'),
                        itemId: 'close'
                    }]
                });
        };

        newWindow.show();
        if (showMessage) {
            App.Notify.pushInfo(l10n.ns('tpm', 'PromoProductsView').value('SavePromoMessage'));
        }
    },

    setEditable: function (editable) {
        this.callParent(arguments);
        if (this.triggerCell && this.inputEl) {
            if (!editable) {
                this.triggerCell.addCls(this.triggerNoEditCls);
                this.inputEl.addCls(this.triggerNoEditCls);
            } else {
                this.triggerCell.removeCls(this.triggerNoEditCls);
                this.inputEl.removeCls(this.triggerNoEditCls);
            }
        }
    },

    setReadOnly: function (readOnly) {
        this.callParent(arguments);
        if (this.triggerCell) {
            if (readOnly) {
                this.triggerCell.addCls(this.triggerNoEditCls)
            } else {
                this.triggerCell.removeCls(this.triggerNoEditCls)
            }
        }
    }
});