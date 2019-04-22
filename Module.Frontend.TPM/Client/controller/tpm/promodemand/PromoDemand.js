Ext.define('App.controller.tpm.promodemand.PromoDemand', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promodemand[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promodemand directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promodemand #datatable': {
                    activate: this.onActivateCard
                },
                'promodemand #detailform': {
                    activate: this.onActivateCard
                },
                'promodemand #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promodemand #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promodemand #detail': {
                    click: this.onDetailButtonClick
                },
                'promodemand #table': {
                    click: this.onTableButtonClick
                },
                'promodemand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promodemand #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promodemand #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promodemand #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promodemand #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promodemand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promodemand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promodemand #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promodemand #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promodemand #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promodemand #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promodemand #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promodemandeditor': {
                    afterrender: this.afterrenderWindowEditor
                },
                'promodemandeditor #edit': {
                    click: this.windowEditorStartEdit
                }
            }
        });
    },

    afterrenderWindowEditor: function (window, eOpts) {
        var editBtn = window.down('#edit');

        if (!editBtn || !editBtn.isVisible())
            this.initMechanicFiled(window);            
    },

    windowEditorStartEdit: function (button) {
        this.initMechanicFiled(button.up('window'));
    },

    initMechanicFiled: function (window) {
        var mechanic = window.down('[name=MechanicId]');
        var mechanicType = window.down('[name=MechanicTypeId]');
        var discount = window.down('[name=Discount]');

        mechanic.addListener('change', this.demandMechanicListener);
        mechanicType.addListener('change', this.demandMechanicTypeListener);

        if (mechanic.rawValue != 'VP')
            mechanicType.setReadOnly(true);
        else
            discount.setReadOnly(true);
    },

    demandMechanicListener: function (field, newValue, oldValue) {
        var mechanicType = field.up().down('[name=MechanicTypeId]');
        var discount = field.up().down('[name=Discount]');

        mechanicType.clearValue();

        if (field.rawValue != 'VP') {
            mechanicType.setReadOnly(true);
            discount.setReadOnly(false);            
        }
        else {
            mechanicType.setReadOnly(false);
            discount.setValue(null);
            discount.setReadOnly(true);            
        }
    },

    demandMechanicTypeListener: function (field, newValue, oldValue) {
        var discount = field.up().down('[name=Discount]');
        var discountValue = newValue != undefined ? field.record.get('Discount') : null;

        discount.setValue(discountValue);
    },
});
