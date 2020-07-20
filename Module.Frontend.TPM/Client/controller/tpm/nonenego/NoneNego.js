Ext.define('App.controller.tpm.nonenego.NoneNego', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nonenego[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'nonenego[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick
                },
                'nonenego directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'nonenego #datatable': {
                    activate: this.onActivateCard
                },
                'nonenego #detailform': {
                    activate: this.onActivateCard
                },
                'nonenego #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'nonenego #detailform #next': {
                    click: this.onNextButtonClick
                },
                'nonenego #detail': {
                    click: this.onDetailButtonClick
                },
                'nonenego #table': {
                    click: this.onTableButtonClick
                },
                'nonenego #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'nonenego #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'nonenego #createbutton': {
                    click: this.onCreateButtonClick,
                },
                'nonenego #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'nonenego #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'nonenego #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'nonenego #refresh': {
                    click: this.onRefreshButtonClick
                },
                'nonenego #close': {
                    click: this.onCloseButtonClick
                },

                // import/export
                'nonenego #exportbutton': {
                    click: this.onExportButtonClick
                },
                'nonenego #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'nonenego #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'nonenego #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'nonenegoeditor': {
                    afterrender: this.afterrenderWindowEditor,
                },
                'nonenegoeditor #edit': {
                    click: this.windowEditorStartEdit
                },
                'nonenegoeditor [name=ClientTreeId]': {
                    change: this.onClientTreeIdChange
                },
                'nonenegoeditor [name=ProductTreeId]': {
                    change: this.onProductTreeIdChange
                },
                'nonenegoeditor [name=FromDate]': {
                    change: this.onFromDateChange
                },
                'nonenegoeditor [name=ToDate]': {
                    select: this.onToDateSelect
                },
                'nonenegoeditor #ok': {
                    click: this.onOkButtonClick
                }
            }
        });
    },

    afterrenderWindowEditor: function (window, eOpts) {
        var me = this;
        var nonenego = Ext.ComponentQuery.query('nonenego')[0];
        var grid = nonenego.down('#datatable');
        var noneNegoId = grid.getSelectionModel().getSelection()[0] && nonenego.isEditorForUpdateNode ? grid.getSelectionModel().getSelection()[0].data.Id : null;
        var nonenegoeditor = Ext.ComponentQuery.query('nonenegoeditor')[0];


        me.elements = {
            clientTreeId: nonenegoeditor.down('[name=ClientTreeId]'),
            clientTreeObjectId: nonenegoeditor.down('[name=ClientTreeObjectId]'),
            productTreeId: nonenegoeditor.down('[name=ProductTreeId]'),
            productTreeObjectId: nonenegoeditor.down('[name=ProductTreeObjectId]'),
            mechanicTypeId: nonenegoeditor.down('[name=MechanicTypeId]'),
            mechanicId: nonenegoeditor.down('[name=MechanicId]'),
            discount: nonenegoeditor.down('[name=Discount]'),
            fromDate: nonenegoeditor.down('[name=FromDate]'),
            toDate: nonenegoeditor.down('[name=ToDate]'),
            createDate: nonenegoeditor.down('[name=CreateDate]'),
            noneNegoId: noneNegoId,
        };

        me.buttons = {
            ok: window.down('#ok'),
            edit: window.down('#edit')
        };

        if (me.buttons.edit) {
            me.buttons.edit.addListener('click', function () {
                me.isEditorForUpdateNode = true;
                me.disableReadOnlyElements([me.elements.createDate]);
                me.addClsElements([me.elements.createDate], 'field-for-read-only');
                me.initMechanicFiled(window);
                nonenegoeditor.storeLoaded = true;

                if (nonenego.isEditorForUpdateNode) {
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                    if (currentRole !== 'SupportAdministrator') {

                        me.elements.fromDate.setMinValue(null);
                    }
                    me.elements.fromDate.valueChanged = false;
                    me.elements.fromDate.isCurrentFieldValid = true;
                    me.elements.toDate.isCurrentFieldValid = true;
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                    if (currentRole !== 'SupportAdministrator') {

                        var date = new Date();
                        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
                        me.elements.fromDate.getPicker().setMinDate(date);

                    }
                }

                me.buttons.cancel = Ext.ComponentQuery.query('#cancel')[0];
                me.buttons.cancel.addListener('click', function () {
                    me.removeClsElements([me.elements.mechanicTypeId, me.elements.discount, me.elements.fromDate,
                    me.elements.toDate, me.elements.createDate], 'field-for-read-only');
                });
            });
        }

        if (!me.buttons.edit || !me.buttons.edit.isVisible()) {
            this.initMechanicFiled(window);
            me.disableReadOnlyElements([me.elements.createDate]);
            //me.addClsElements([me.elements.createDate], 'field-for-read-only');
        }

        if (!me.elements.clientTreeId.getValue() && !me.elements.productTreeId.getValue()) {
            me.disableReadOnlyElements([me.elements.fromDate, me.elements.toDate]);
            me.addClsElements([me.elements.fromDate, me.elements.toDate], 'field-for-read-only');
        }

        if (nonenego.isEditorForUpdateNode) {
            me.addClsElements([me.elements.createDate], 'field-for-read-only');
        }

        if (!nonenego.isEditorForUpdateNode && !me.buttons.edit) {
            me.addClsElements([me.elements.mechanicTypeId, me.elements.discount, me.elements.createDate], 'field-for-read-only');
        }
    },

    windowEditorStartEdit: function (button) {
        var window = button.up('window');
        this.initMechanicFiled(window);
    },

    initMechanicFiled: function (window) {
        var me = this;
        me.elements.clientTreeId.addListener('change', this.nonenegoClientListener);
        me.elements.mechanicId.addListener('change', this.nonenegoMechanicListener);
        me.elements.mechanicTypeId.addListener('change', this.nonenegoMechanicTypeListener);
        me.elements.clientTreeId.checkChange();
        me.elements.mechanicId.checkChange();
        me.elements.mechanicTypeId.checkChange();

        if (me.elements.clientTreeId.rawValue != '' && me.elements.clientTreeId.rawValue != null) {
            if (me.elements.mechanicId.rawValue != 'VP') {
                me.disableReadOnlyElements([me.elements.mechanicTypeId]);
                if (Ext.ComponentQuery.query('nonenego')[0].isEditorForUpdateNode) {
                    me.enableReadOnlyElements([me.elements.discount]);
                }

                me.addClsElements([me.elements.mechanicTypeId], 'field-for-read-only');
                me.removeClsElements([me.elements.discount], 'field-for-read-only');
            }
            else {
                me.enableReadOnlyElements([me.elements.mechanicTypeId]);
                me.disableReadOnlyElements([me.elements.discount]);

                me.removeClsElements([me.elements.mechanicTypeId], 'field-for-read-only');
                me.addClsElements([me.elements.discount], 'field-for-read-only');

                var nonegoController = App.app.getController('tpm.nonenego.NoneNego');
                nonegoController.getMechanicTypesByClient(window);
            }
        } else {
            me.disableReadOnlyElements([me.elements.discount, me.elements.mechanicId, me.elements.mechanicTypeId]);
            me.addClsElements([me.elements.discount, me.elements.mechanicId, me.elements.mechanicTypeId], 'field-for-read-only');
        }
    },

    nonenegoClientListener: function (field, newValue, oldValue) {
        var nonenegoeditor = field.up('nonenegoeditor');
        var mechanic = nonenegoeditor.down('[name=MechanicId]');

        if (nonenegoeditor.storeLoaded) {
            mechanic.clearValue();

            if (field.rawValue != '' && field.rawValue != null) {
                mechanic.removeCls('field-for-read-only');

                mechanic.setReadOnly(false);
            }
            else {
                mechanic.addCls('field-for-read-only');

                mechanic.setValue(null);
                mechanic.setReadOnly(true);
            }

            var me = App.app.getController('tpm.nonenego.NoneNego');
            me.validateFields(me);
        }
        nonenegoeditor.storeLoaded = true;
    },

    getMechanicTypesByClient: function (nonenegoeditor) {
        var nonegoController = App.app.getController('tpm.nonenego.NoneNego');
        var client = nonenegoeditor.down('[name=ClientTreeId]');
        mechanicFields = nonegoController.getMechanicFields(nonenegoeditor);
        store = mechanicFields.mechanicTypeId.getStore();

        var fieldValue = mechanicFields.mechanicId.rawValue;

        if (fieldValue == nonegoController.getMechanicListForUnlockDiscountField()) {
            var record = client.value;

            if (record !== null) {
                store.getProxy().extraParams = {
                    byClient: breeze.DataType.Boolean.fmtOData('true'),
                    clientTreeId: breeze.DataType.Int32.fmtOData(record),
                };
            }
        }
    },

    getMechanicListForUnlockDiscountField: function () {
        return ['VP'];
    },

    getMechanicFields: function (nonenegoeditor) {
        return {
            mechanicId: nonenegoeditor.down('searchfield[name=MechanicId]'),
            mechanicTypeId: nonenegoeditor.down('searchcombobox[name=MechanicTypeId]'),
            mechanicDiscount: nonenegoeditor.down('numberfield[name=Discount]')
        }
    },

    nonenegoMechanicListener: function (field, newValue, oldValue) {
        var me = App.app.getController('tpm.nonenego.NoneNego');
        if (!me.buttons.edit || field.firstChange) {
            var nonenegoeditor = field.up('nonenegoeditor');
            var mechanicType = nonenegoeditor.down('[name=MechanicTypeId]');
            var discount = nonenegoeditor.down('[name=Discount]');

            if (nonenegoeditor.storeLoaded) {
                mechanicType.clearValue();

                if (field.rawValue != '' && field.rawValue != null) {
                    if (field.rawValue != 'VP') {
                        mechanicType.addCls('field-for-read-only');
                        discount.removeCls('field-for-read-only');

                        mechanicType.setReadOnly(true);
                        discount.setReadOnly(false);
                    }
                    else {
                        mechanicType.removeCls('field-for-read-only');
                        discount.addCls('field-for-read-only');

                        mechanicType.setReadOnly(false);
                        discount.setValue(0);
                        discount.setReadOnly(true);

                        var nonegoController = App.app.getController('tpm.nonenego.NoneNego');
                        nonegoController.getMechanicTypesByClient(nonenegoeditor);
                    }
                } else {
                    mechanicType.addCls('field-for-read-only');
                    mechanicType.setReadOnly(true);

                    discount.addCls('field-for-read-only');
                    discount.setValue(0);
                    discount.setReadOnly(true);
                }

                var me = App.app.getController('tpm.nonenego.NoneNego');
                me.validateFields(me);
            }
            nonenegoeditor.storeLoaded = true;
        }
        field.firstChange = true;
    },

    nonenegoMechanicTypeListener: function (field, newValue, oldValue) {
        var me = App.app.getController('tpm.nonenego.NoneNego');
        if (!me.buttons.edit || field.firstChange) {
            var discount = field.up('nonenegoeditor').down('[name=Discount]');
            var discountValue = newValue != undefined ? field.record.get('Discount') : 0;

            discount.setValue(discountValue);

            me.validateFields(me);
        }
        field.firstChange = true;
    },

    onClientTreeIdChange: function () {
        if (this.elements.clientTreeId.getModelData().ClientTreeObjectId)
            this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
        this.validateFields();
    },
    
    onProductTreeIdChange: function () {
        this.elements.productTreeObjectId.setValue(this.elements.productTreeId.getModelData().ProductTreeObjectId);
        this.validateFields();
    },

    onFromDateChange: function (fromDate, newValue, oldValue) {
        var me = this;
        if (newValue != oldValue) {
            if (fromDate.firstChange) {
                fromDate.firstChange = false;
            } else {
                fromDate.valueChanged = true;
                var minValue = new Date();
                var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                var minValueInt = minValue.getTime();
                var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                if (currentRole !== 'SupportAdministrator') {

                    fromDate.setMinValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                }
                me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId, me.elements.mechanicTypeId, me.elements.discount);
            }
        }
    },

    onToDateSelect: function () {
        var me = this;
        if (me.elements.fromDate.getValue()) {
            me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId, me.elements.mechanicTypeId, me.elements.discount);
        }
    },

    onOkButtonClick: function () {
        var me = this;
    },

    enableReadOnlyElements: function (elements) {
        elements.forEach(function (x) {
            x.setReadOnly(false);
        });
    },

    disableReadOnlyElements: function (elements) {
        elements.forEach(function (x) {
            x.setReadOnly(true);
        });
    },

    addClsElements: function (elements, cls) {
        elements.forEach(function (x) {
            x.addCls(cls);
        });
    },

    removeClsElements: function (elements, cls) {
        elements.forEach(function (x) {
            x.removeCls(cls);
        });
    },

    validateFields: function (scope) {
        var me = scope || this;

        if (me.elements.clientTreeId.getValue() && me.elements.productTreeId.getValue() && me.elements.mechanicId.getValue()) {
            me.enableReadOnlyElements([me.elements.fromDate, me.elements.toDate]);
            me.removeClsElements([me.elements.fromDate, me.elements.toDate], 'field-for-read-only');
            if (me.elements.fromDate.valueChanged) {
                me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId, me.elements.mechanicTypeId, me.elements.discount);
            }

        } else {
            me.disableReadOnlyElements([me.elements.fromDate, me.elements.toDate]);
            me.addClsElements([me.elements.fromDate, me.elements.toDate], 'field-for-read-only');
        }
    },

    validatePeriod: function (fromDate, toDate, noneNegoId, clientTreeId, productTreeId, mechanic, mechanicType, discount) {
        var me = this;
        if (!fromDate.isDisabled()) {
            var nonenegoeditor = Ext.ComponentQuery.query('nonenegoeditor')[0];
            var myMask = new Ext.LoadMask(nonenegoeditor, { msg: "Please wait..." });
            myMask.show();

            var parameters = {
                fromDate: breeze.DataType.DateTimeOffset.fmtOData(changeTimeZone(fromDate.getValue(), 3, -1)),
                toDate: breeze.DataType.DateTimeOffset.fmtOData(changeTimeZone(toDate.getValue(), 3, -1)),
                noneNegoId: noneNegoId,
                clientTreeId: clientTreeId.getValue(),
                productTreeId: productTreeId.getValue(),
                mechanicId: mechanic.getValue(),
                mechanicTypeId: mechanicType.getValue(),
                discount: discount.getValue()
            };

            var correctMechanic = parameters.mechanicId != undefined && parameters.mechanicId != null && parameters.mechanicId != '';
            if (parameters.productTreeId != undefined && parameters.clientTreeId != undefined && correctMechanic) {
                App.Util.makeRequestWithCallback('NoneNegoes', 'IsValidPeriod', parameters, function (data) {
                    if (data) {
                        var nonenegoeditor = Ext.ComponentQuery.query('nonenegoeditor')[0];
                        if (nonenegoeditor) {
                            var fromDate = nonenegoeditor.down('[name=FromDate]');
                            var toDate = nonenegoeditor.down('[name=ToDate]');

                            var result = Ext.JSON.decode(data.httpResponse.data.value);
                            if (result.success) {
                                fromDate.isCurrentFieldValid = true;
                                toDate.isCurrentFieldValid = true;

                                fromDate.clearInvalid();
                                toDate.clearInvalid();
                                fromDate.validate();
                                toDate.validate();
                            } else {
                                fromDate.isCurrentFieldValid = false;
                                toDate.isCurrentFieldValid = false;

                                fromDate.markInvalid(l10n.ns('tpm', 'NoneNego').value('ValidatePeriodError'));
                                toDate.markInvalid(l10n.ns('tpm', 'NoneNego').value('ValidatePeriodError'));
                            }
                        }

                        myMask.hide();
                    }
                }, function (data) {
                    App.Notify.pushError(data.message);
                    window.setLoading(false);
                });
            }
        }
    },

    onGridAfterrender: function (grid, eOpts) {
        var store = grid.getStore();
        var extendedFilter = store.getExtendedFilter();
        // выравниваем дату по UTC
        var currentTime = new Date();
        var dateFilter = new Date(currentTime.getFullYear(), currentTime.getMonth(), currentTime.getDate(), 0, 0, 0, 0);

        extendedFilter.activeModel.entries.items[4].data.value = Ext.create('App.extfilter.core.Range', dateFilter, null);
        extendedFilter.filter = this.getDefaultFilter(dateFilter);

        extendedFilter.clear = function (suppressReload) {
            var model = this.getFilterModel();

            if (!model) {
                return;
            }

            var currentTime = new Date();
            var dateFilter = new Date(currentTime.getFullYear(), currentTime.getMonth(), currentTime.getDate(), 0, 0, 0, 0);
            var f = function (dateFilter) {
                var r = {
                    operator: 'and',
                    rules: [{
                        operator: 'and',
                        rules: [{
                            property: 'ToDate',
                            operation: 'GreaterOrEqual',
                            value: dateFilter
                        }]
                    }]
                }
                return r;
            };

            model.clear();

            this.activeModel.entries.items[4].data.value = Ext.create('App.extfilter.core.Range', dateFilter, null);
            this.filter = f(dateFilter);

            this.reloadStore(suppressReload);
            this.fireEvent('extfilterchange', this);
        }

        this.callParent(arguments);
    },

    getDefaultFilter: function (dateFilter) {
        // отбирает записи с ToDate >= Сегодня
        var filter = {
            operator: 'and',
            rules: [{
                operator: 'and',
                rules: [{
                    property: 'ToDate',
                    operation: 'GreaterOrEqual',
                    value: dateFilter
                }]
            }]
        };

        return filter;
    },

    onCreateButtonClick: function () {
        this.callParent(arguments);

        var nonenegoeditor = Ext.ComponentQuery.query('nonenegoeditor')[0];
        var createDate = nonenegoeditor.down('[name=CreateDate]');
        var fromDate = nonenegoeditor.down('[name=FromDate]');
        var toDate = nonenegoeditor.down('[name=ToDate]');

        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);   // приведение к московской timezone
        createDate.setValue(date);
        fromDate.getPicker().setValue(date);
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (currentRole !== 'SupportAdministrator') {

            fromDate.setMinValue(date);
        }
        fromDate.valueChanged = true;
        fromDate.isCurrentFieldValid = false;
        toDate.isCurrentFieldValid = false;
        nonenegoeditor.storeLoaded = true;
    },
});