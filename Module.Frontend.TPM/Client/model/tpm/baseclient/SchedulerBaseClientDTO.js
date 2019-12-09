Ext.define('App.model.tpm.baseclient.SchedulerClientTreeDTO', {
    extend: 'Sch.model.Resource',
    mixins: ['Ext.data.Model'],
    idProperty: 'InOutId',
    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'GHierarchyCode', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'Share', type: 'int' },

        { name: 'IsBeforeStart', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysStart', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysStart', type: 'int', hidden: false, useNull: true },

        { name: 'IsBeforeEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysEnd', type: 'int', hidden: false, useNull: true },
        { name: 'TypeName', type: 'string', hidden: false, useNull: true },
        { name: 'InOutId', type: 'string', hidden: true}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'SchedulerClientTreeDTOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
    getEvents: function (store) {
        var key = (this.get('ObjectId') || this.internalId).toString(),
            ClientType = (this.get('TypeName')).toLowerCase(),
            results = [],
            notOtherTypes = ["regular", "inout"];
        if (store) {
            if (notOtherTypes.includes(ClientType)) {
                for (var index = 0, count = store.getCount(); index < count; index++) {
                    var record = store.getAt(index);
                    var bcids = record.get('BaseClientTreeIds');
                    var PromoType = record.get('TypeName').toLowerCase();
                    if (bcids) {
                        var bIDs = bcids.split('|');
                        if (bIDs.indexOf(key) >= 0 && ClientType == PromoType) {
                            results.push(record);
                        }
                    }
                }
            } else {
                for (var index = 0, count = store.getCount(); index < count; index++) {
                    var record = store.getAt(index);
                    var bcids = record.get('BaseClientTreeIds');
                    var PromoType = record.get('TypeName').toLowerCase();
                    if (bcids) {
                        var bIDs = bcids.split('|');
                        if (bIDs.indexOf(key) >= 0 && !notOtherTypes.includes(PromoType)) {
                            results.push(record);
                        }
                    }
                }
            }
        }
        return results;
    }
});