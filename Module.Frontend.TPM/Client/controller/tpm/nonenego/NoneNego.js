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
                    select: this.onFromDateSelect
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

        me.elements.mechanicId.addListener('change', this.nonenegoMechanicListener);
        me.elements.mechanicTypeId.addListener('change', this.nonenegoMechanicTypeListener);

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
        }
    },

    nonenegoMechanicListener: function (field, newValue, oldValue) {
        var mechanicType = field.up('nonenegoeditor').down('[name=MechanicTypeId]');
        var discount = field.up('nonenegoeditor').down('[name=Discount]');        

        mechanicType.clearValue();

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
            discount.setValue(null);
            discount.setReadOnly(true);
        }

        var me = App.app.getController('tpm.nonenego.NoneNego');
        me.validateFields(me);
    },

    nonenegoMechanicTypeListener: function (field, newValue, oldValue) {
        var discount = field.up('nonenegoeditor').down('[name=Discount]');
        var discountValue = newValue != undefined ? field.record.get('Discount') : null;

        discount.setValue(discountValue);
    },

    onClientTreeIdChange: function () {
        this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
        this.validateFields();
    },

    onProductTreeIdChange: function () {        
        this.elements.productTreeObjectId.setValue(this.elements.productTreeId.getModelData().ProductTreeObjectId);
        this.validateFields();
    },

    onFromDateSelect: function () {
        var me = this;
        me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId);
    },

    onToDateSelect: function () {
        var me = this;
        if (me.elements.fromDate.getValue()) {
            me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId);
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

        if (!me.elements.fromDate.getValue()) {
            me.elements.fromDate.setValue(new Date());
        }

        if (me.elements.clientTreeId.getValue() && me.elements.productTreeId.getValue() && me.elements.mechanicId.getValue()) {
            me.enableReadOnlyElements([me.elements.fromDate, me.elements.toDate]);
            me.removeClsElements([me.elements.fromDate, me.elements.toDate], 'field-for-read-only');
            me.validatePeriod(me.elements.fromDate, me.elements.toDate, me.elements.noneNegoId, me.elements.clientTreeId, me.elements.productTreeId, me.elements.mechanicId);
        } else {
            me.disableReadOnlyElements([me.elements.fromDate, me.elements.toDate]);
            me.addClsElements([me.elements.fromDate, me.elements.toDate], 'field-for-read-only');
        }
    },

    validatePeriod: function (fromDate, toDate, noneNegoId, clientTreeId, productTreeId, mechanic) {
        var me = this;
        if (!fromDate.isDisabled()) {
            var nonenegoeditor = Ext.ComponentQuery.query('nonenegoeditor')[0];
            var myMask = new Ext.LoadMask(nonenegoeditor, { msg: "Please wait..." });
            myMask.show();

            var parameters = {
                fromDate: breeze.DataType.DateTimeOffset.fmtOData(fromDate.getValue()),
                toDate: breeze.DataType.DateTimeOffset.fmtOData(toDate.getValue()),
                noneNegoId: noneNegoId,
                clientTreeId: clientTreeId.getValue(),
                productTreeId: productTreeId.getValue(),
                mechanicId: mechanic.getValue()
            };

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
    },

    onGridAfterrender: function (grid, eOpts) {
        var store = grid.getStore(); 
        var extendedFilter = store.getExtendedFilter();
        // выравниваем дату по UTC
        var currentTime = new Date();
        var dateFilter = new Date(currentTime.getFullYear(), currentTime.getMonth(), currentTime.getDate(), 0, 0, 0, 0);

        extendedFilter.activeModel.entries.items[4].data.value = Ext.create('App.extfilter.core.Range', dateFilter, null);
        extendedFilter.filter = this.getDefaultFilter(dateFilter);

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
                    operation: 'GraterOrEqual',
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
        createDate.setValue(new Date());
    },
});