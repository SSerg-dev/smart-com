Ext.define('App.controller.tpm.baseclienttreeview.BaseClientTreeView', {
    extend: 'App.controller.core.AssociatedDirectory',
    //mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'baseclienttreeview[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'baseclienttreeview[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'baseclienttreeview directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'baseclienttreeview #datatable': {
                    activate: this.onActivateCard
                },
                'baseclienttreeview #detailform': {
                    activate: this.onActivateCard
                },
                'baseclienttreeview #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'baseclienttreeview #detailform #next': {
                    click: this.onNextButtonClick
                },
                'baseclienttreeview #detail': {
                    click: this.onDetailButtonClick
                },
                'baseclienttreeview #table': {
                    click: this.onTableButtonClick
                },
                'baseclienttreeview #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'baseclienttreeview #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'baseclienttreeview #createbutton': {
                    click: this.onCreateButtonClick
                },
                'baseclienttreeview #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'baseclienttreeview #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'baseclienttreeview #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'baseclienttreeview #refresh': {
                    click: this.onRefreshButtonClick
                },
                'baseclienttreeview #close': {
                    click: this.onCloseButtonClick
                },
                //// import/export
                //'baseclienttreeview #exportbutton': {
                //    click: this.onExportButtonClick
                //},
                //'baseclienttreeview #loadimportbutton': {
                //    click: this.onShowImportFormButtonClick
                //},
                //'baseclienttreeview #loadimporttemplatebutton': {
                //    click: this.onLoadImportTemplateButtonClick
                //},
                //'baseclienttreeview #applyimportbutton': {
                //    click: this.onApplyImportButtonClick
                //}
            }
        });
    },
});