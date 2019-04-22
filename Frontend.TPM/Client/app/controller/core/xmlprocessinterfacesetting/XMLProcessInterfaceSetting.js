Ext.define('App.controller.core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'xmlprocessinterfacesetting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'xmlprocessinterfacesetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'xmlprocessinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'xmlprocessinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'xmlprocessinterfacesetting #detailform #ok': {
                    click: this.onOkButtonClick
                },
                'xmlprocessinterfacesetting #detailform #cancel': {
                    click: this.onCancelButtonClick
                },
                'xmlprocessinterfacesetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'xmlprocessinterfacesetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'xmlprocessinterfacesetting #detail': {
                    click: this.switchToDetailForm
                },
                'xmlprocessinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'xmlprocessinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'xmlprocessinterfacesetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'xmlprocessinterfacesetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'xmlprocessinterfacesetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'xmlprocessinterfacesetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'xmlprocessinterfacesetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'xmlprocessinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'xmlprocessinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});