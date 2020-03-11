Ext.define('App.controller.core.csvprocessinterfacesetting.CSVProcessInterfaceSetting', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'csvprocessinterfacesetting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'csvprocessinterfacesetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'csvprocessinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'csvprocessinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'csvprocessinterfacesetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'csvprocessinterfacesetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'csvprocessinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'csvprocessinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'csvprocessinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'csvprocessinterfacesetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'csvprocessinterfacesetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'csvprocessinterfacesetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'csvprocessinterfacesetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'csvprocessinterfacesetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'csvprocessinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'csvprocessinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },

});