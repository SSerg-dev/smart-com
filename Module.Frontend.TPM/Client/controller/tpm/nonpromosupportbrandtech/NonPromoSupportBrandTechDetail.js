Ext.define('App.controller.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nonpromosupportbrandtechdetail directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    itemdblclick: this.onDetailButtonClick
                },
                'nonpromosupportbrandtechdetail #datatable': {
                    activate: this.onActivateCard
                },
                'nonpromosupportbrandtechdetail #detailform': {
                    activate: this.onActivateCard
                },
                'nonpromosupportbrandtechdetail #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'nonpromosupportbrandtechdetail #detailform #next': {
                    click: this.onNextButtonClick
                },
                'nonpromosupportbrandtechdetail #detail': {
                    click: this.onDetailButtonClick
                },
                'nonpromosupportbrandtechdetail #table': {
                    click: this.onTableButtonClick
                },
                'nonpromosupportbrandtechdetail #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'nonpromosupportbrandtechdetail #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'nonpromosupportbrandtechdetail #addbutton': {
                    click: this.onAddButtonClick
                },
                'nonpromosupportbrandtechdetail #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'nonpromosupportbrandtechdetail #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'nonpromosupportbrandtechdetail #refresh': {
                    click: this.onRefreshButtonClick
                },
                'nonpromosupportbrandtechdetail #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'nonpromosupportbrandtechdetail #exportbutton': {
                    click: this.onExportButtonClick
                },
                'nonpromosupportbrandtechdetail #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'nonpromosupportbrandtechdetail #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'nonpromosupportbrandtechdetail #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onAddButtonClick: function (button) {
        var widget = Ext.widget('nonpromosupportbrandtechchoose');
        widget.nonPromoSupportBrandTechDetailGrid = button.up('nonpromosupportbrandtechdetail').down('grid');
        widget.associatedGrid = true;
        widget.selectedNonPromoSupportRecordId = null;
        widget.down('brandtech').down('directorygrid').multiSelect = true;

        var nonPromoSupportGrid = button.up('#associatednonpromosupportcontainer').down('nonpromosupport').down('grid'),
            selModel = nonPromoSupportGrid.getSelectionModel(),
            selectedNonPromoSupportRecord;

        if (selModel.hasSelection()) {
            var selectedNonPromoSupportRecord = selModel.getSelection()[0];
            widget.selectedNonPromoSupportRecordId = selectedNonPromoSupportRecord.data.Id;
        }

        var nonPromoSupportBrandTechDetailGrid = button.up('panel').down('grid');

        if (nonPromoSupportBrandTechDetailGrid) {
            var nonPromoSupportBrandTechDetailStore = nonPromoSupportBrandTechDetailGrid.getStore(),
                count = nonPromoSupportBrandTechDetailStore.getCount(),
                nonPromoSupportBrandTechDetailRecords = count > 0 ? nonPromoSupportBrandTechDetailStore.getRange(0, count) : [],
                nonPromoSupportBrandTechDetailChooseGrid = widget.down('grid'),
                nonPromoSupportBrandTechDetailChooseStore = nonPromoSupportBrandTechDetailChooseGrid.getStore();

             nonPromoSupportBrandTechDetailChooseStore.on({
                scope: this,
                load: function () {
                    var checkedRows = [];

                    nonPromoSupportBrandTechDetailRecords.forEach(function (checkedRow) {
                        var isRecordsChecked = nonPromoSupportBrandTechDetailChooseStore.findRecord('Id', checkedRow.data.BrandTechId);
                        if (isRecordsChecked) {
                            checkedRows.push(isRecordsChecked);
                        }
                    });

                    nonPromoSupportBrandTechDetailChooseGrid.getSelectionModel().checkRows(checkedRows);
                },
                single: true
            });
        }

        widget.show();
    },

    onDeleteButtonClick: function (button) {
        var grid = button.up('nonpromosupportbrandtechdetail').down('directorygrid');

        if (grid && grid.gridTotalCount === 1) {
            Ext.Msg.show({
                title: l10n.ns('core').value('errorTitle'),
                msg: l10n.ns('tpm', 'NonPromoSupportBrandTechDetail').value('EmptyBrandTechErrorMsg'),
                buttons: Ext.MessageBox.OK,
                icon: Ext.Msg.ERROR
            });
        } else {
            var panel = grid.up('combineddirectorypanel'),
                selModel = grid.getSelectionModel();

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
                    var record = selModel.getSelection()[0],
                        store = grid.getStore(),
                        view = grid.getView(),
                        currentIndex = store.indexOf(record),
                        pageIndex = store.getPageFromRecordIndex(currentIndex),
                        endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                    grid.gridTotalCount--;

                    currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                    panel.setLoading(l10n.ns('core').value('deletingText'));

                    record.destroy({
                        scope: this,
                        success: function () {
                            if (view.bufferedRenderer) {
                                selModel.deselectAll();
                                //selModel.clearSelections();
                                store.data.removeAtKey(pageIndex);
                                store.totalCount--;

                                if (store.getTotalCount() > 0) {
                                    view.bufferedRenderer.scrollTo(currentIndex, true, function () {
                                        panel.setLoading(false);
                                    });
                                } else {
                                    grid.setLoadMaskDisabled(true);
                                    store.on({
                                        single: true,
                                        load: function (records, operation, success) {
                                            panel.setLoading(false);
                                            grid.setLoadMaskDisabled(false);
                                        }
                                    });
                                    store.load();
                                }
                            } else {
                                panel.setLoading(false);
                            }
                        },
                        failure: function () {
                            panel.setLoading(false);
                        }
                    });
                }
            }
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('nonpromosupportbrandtechdetail').down('directorygrid');
        selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            this.startDetailRecord(selModel.getSelection()[0], grid);
        } else {
            console.log('No selection');
        }
    },

    startDetailRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
        this.editor.model = model;
        this.editor.grid = grid;

        this.editor.down('[name=BrandId]').setRawValue(model.data.BrandTechBrandName);
        this.editor.down('[name=TechnologyId]').setRawValue(model.data.BrandTechTechnologyName);

        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(true);
        }, this);

        this.editor.down('#ok').setVisible(false);
        this.editor.down('#edit').setVisible(false);
        this.editor.down('#canceledit').setVisible(false);

        this.editor.down('#close').on('click', this.onCloseButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();
    },

    onCloseButtonClick: function (button) {
        button.up('window').close();
    },

    onEditorClose: function (window) {
        var form = this.editor.down('editorform'),
            record = form.getRecord();

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },
})