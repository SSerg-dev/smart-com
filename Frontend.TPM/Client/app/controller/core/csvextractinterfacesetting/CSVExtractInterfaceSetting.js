Ext.define('App.controller.core.csvextractinterfacesetting.CSVExtractInterfaceSetting', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'csvextractinterfacesetting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'csvextractinterfacesetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'csvextractinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'csvextractinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'csvextractinterfacesetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'csvextractinterfacesetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'csvextractinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'csvextractinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'csvextractinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'csvextractinterfacesetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'csvextractinterfacesetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'csvextractinterfacesetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'csvextractinterfacesetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'csvextractinterfacesetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'csvextractinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'csvextractinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }

});