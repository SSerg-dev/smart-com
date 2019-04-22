Ext.define('App.store.tpm.mechanic.MechanicTypeStore', {
    extend: 'App.store.core.SimpleStore',
    autoLoad: false,
    alias: 'store.promoformmechanictypestore',
    model: 'App.model.tpm.mechanictype.MechanicType',
    extendedFilter: {
        xclass: 'App.ExtFilterContext',
        supportedModels: [{
            xclass: 'App.ExtSelectionFilterModel',
            model: 'App.model.tpm.mechanictype.MechanicType',
            modelId: 'efselectionmodel'
        }]
    }
});
