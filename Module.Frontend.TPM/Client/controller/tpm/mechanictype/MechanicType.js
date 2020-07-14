Ext.define('App.controller.tpm.mechanictype.MechanicType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'mechanictype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'mechanictype #datatable': {
                    activate: this.onActivateCard
                },
                'mechanictype #detailform': {
                    activate: this.onActivateCard
                },
                'mechanictype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'mechanictype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'mechanictype #detail': {
                    click: this.onDetailButtonClick
                },
                'mechanictype #table': {
                    click: this.onTableButtonClick
                },
                'mechanictype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'mechanictype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'mechanictype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'mechanictype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'mechanictype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'mechanictype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'mechanictype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'mechanictype #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'mechanictype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'mechanictype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'mechanictype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'mechanictype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

            }
        });
    },

    onTrigger1Click: function (picker) {
        var picker = picker.createPicker();
        var mechanicTypeController = App.app.getController('tpm.mechanictype.MechanicType');
        var clientTreeStore = mechanicTypeController.getClientTreeStore();

        var clientTree = picker.down(this.selectorWidget);
        var clientTreeGrid = clientTree.down('baseclienttreesearchfield');

        clientTreeGrid.addListener('select', mechanicTypeController.onTreeNodeSelect);

        picker.show();
        var addNodeButton = clientTree.down('#addNode');
        var deleteNodeButton = clientTree.down('#deleteNode');

        addNodeButton.hide();
        deleteNodeButton.hide();
    },

    onTreeNodeSelect: function (cell, record, item, index, e, eOpts) {
        var treegrid = record.store.ownerTree;
        var clientTree = treegrid.up('clienttree');
        var clientChooseWindow = clientTree.up('window');
        var chooseButton = clientChooseWindow.down('#select');

        if (record.data.IsBaseClient == false) {
            chooseButton.disable();
        }
    },

    getClientTreeStore: function () {
        var clientTreeStore = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.clienttree.ClientTree',
        });

        return clientTreeStore;
    },

});
