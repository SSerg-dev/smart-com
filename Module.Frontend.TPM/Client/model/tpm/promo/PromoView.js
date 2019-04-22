Ext.define('App.model.tpm.promo.PromoView', {
    extend: 'Sch.model.Event',
    mixins: ['Ext.data.Model'],
    idProperty: 'Id',
    breezeEntityType: 'PromoView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTechName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'EventName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsMechanicName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsMechanicDiscount', type: 'int', hidden: false, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: false },
        { name: 'ColorSystemName', type: 'string', useNull: true, hidden: true, isDefault: false },
        { name: 'PromoStatusColor', type: 'string', hidden: true, isDefault: false },

        { name: 'CreatorId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'BaseClientTreeIds', useNull: true, hidden: true, isDefault: false, defaultValue: null },      
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false },
        { name: 'CalendarPriority', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true },
        { name: 'PromoStatusSystemName', type: 'string', hidden: false, isDefault: true },

        //Дублирование встроенных полей Schedule для фильтрации списка полей фильтрации
        { name: "Draggable", type: "boolean", persist: false, hidden: true, defaultValue: true },
        { name: "Resizable", persist: false, hidden: true, defaultValue: true },
        { name: "Cls", hidden: true },
        { name: "ResourceId", hidden: true },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
