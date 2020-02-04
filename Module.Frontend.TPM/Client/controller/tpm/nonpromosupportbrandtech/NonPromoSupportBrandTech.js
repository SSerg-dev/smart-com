Ext.define('App.controller.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTech', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nonpromosupportbrandtech[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'nonpromosupportbrandtech directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange              
                },
                'nonpromosupportbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'nonpromosupportbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'nonpromosupportbrandtech #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'nonpromosupportbrandtech #detailform #next': {
                    click: this.onNextButtonClick
                },
                'nonpromosupportbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'nonpromosupportbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'nonpromosupportbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'nonpromosupportbrandtech #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'nonpromosupportbrandtech #addbutton': {
                    click: this.onAddButtonClick
                },
                'nonpromosupportbrandtech #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'nonpromosupportbrandtech #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'nonpromosupportbrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'nonpromosupportbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'nonpromosupportbrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },

    onAddButtonClick: function (button) {
        var widget = Ext.widget('nonpromosupportbrandtechchoose');
        widget.nonPromoSupportBrandTechGrid = button.up('nonpromosupportbrandtech').down('grid');
        widget.associatedGrid = false;
        widget.down('brandtech').down('directorygrid').multiSelect = true;

        var nonPromoSupportBrandTechGrid = button.up('panel').down('grid');

        if (nonPromoSupportBrandTechGrid) {
            var nonPromoSupportBrandTechStore = nonPromoSupportBrandTechGrid.getStore(),
                count = nonPromoSupportBrandTechStore.getCount(),
                nonPromoSupportBrandTechRecords = count > 0 ? nonPromoSupportBrandTechStore.getRange(0, count) : [],
                nonPromoSupportBrandTechChooseGrid = widget.down('grid'),
                nonPromoSupportBrandTechChooseStore = nonPromoSupportBrandTechChooseGrid.getStore();

            nonPromoSupportBrandTechChooseStore.on({
                scope: this,
                load: function () {
                    var checkedRows = [];

                    nonPromoSupportBrandTechRecords.forEach(function (checkedRow) {
                        var isRecordsChecked = nonPromoSupportBrandTechChooseStore.findRecord('Id', checkedRow.data.BrandTechId);
                        if (isRecordsChecked)
                            checkedRows.push(isRecordsChecked);
                    });

                    nonPromoSupportBrandTechChooseGrid.getSelectionModel().checkRows(checkedRows);
                },
                single: true
            });
        }

        widget.show();
    },

    onDeleteButtonClick: function (button) {
        var nonPromoSupportBrandTechGrid = button.up('panel').down('grid'),
            selModel = nonPromoSupportBrandTechGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('delete'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {
            if (buttonId === 'yes') {
                var nonPromoSupportBrandTechStore = nonPromoSupportBrandTechGrid.getStore(),
                    nonPromoSupportBrandTechProxy = nonPromoSupportBrandTechStore.getProxy(),
                    record = selModel.getSelection()[0];

                var index = nonPromoSupportBrandTechProxy.data.indexOf(record);
                nonPromoSupportBrandTechProxy.data.splice(index, 1);
                nonPromoSupportBrandTechStore.load();
            }
        }
    },

    onGridAfterrender: function (grid) {
        var store = grid.getStore();
        store.addListener('load', function () {
            if (store.getCount() > 0) {
                grid.getSelectionModel().select(store.getAt(0));
            }
        });

        this.callParent(arguments);
    },

    onDetailButtonClick: function (button) {
        // Empty :(
    },
})