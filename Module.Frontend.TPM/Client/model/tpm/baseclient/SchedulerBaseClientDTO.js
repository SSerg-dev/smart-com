Ext.define('App.model.tpm.baseclient.SchedulerClientTreeDTO', {
    extend: 'Sch.model.Resource',
    mixins: ['Ext.data.Model'],
    idProperty: 'Id',
    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'GHierarchyCode', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'CompetitorName', type: 'string', hidden: false, isDefault: true },
        { name: 'Share', type: 'int' },

        { name: 'IsBeforeStart', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysStart', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysStart', type: 'int', hidden: false, useNull: true },

        { name: 'IsBeforeEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysEnd', type: 'int', hidden: false, useNull: true },
        { name: 'TypeName', type: 'string', hidden: false, useNull: true },

        { name: 'IsOnInvoice', type: 'bool', useNull: true, defaultValue: null },
        { name: 'InOutId', type: 'string', hidden: true },
        {name: 'DeviationCoefficient', type: 'float', hidden: false, isDefault: true}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'SchedulerClientTreeDTOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            NoSettings: true
        }
    },
    getEvents: function (store) {
        var key = (this.get('ObjectId') || this.internalId).toString(),
            ClientType = (this.get('TypeName')).toLowerCase(),
            CompetitorName = (this.get('CompetitorName')).toLowerCase(),
            results = [],
            notOtherTypes = ["regular", "inout"];
            otherTypes = ["loyalty", "dynamic"];
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
                if (ClientType == 'competitor') {
                    for (var index = 0, count = store.getCount(); index < count; index++) {
                        var record = store.getAt(index);
                        var bcids = record.get('BaseClientTreeIds');
                        var PromoType = record.get('TypeName').toLowerCase();
                        if (PromoType == ClientType) {
                            var PromoCompetitor = record.get('CompetitorName').toLowerCase();
                            if (bcids) {
                                var bIDs = bcids.split('|');
                                if (bIDs.indexOf(key) >= 0 && CompetitorName == PromoCompetitor) {
                                    results.push(record);
                                }
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
                            if (bIDs.indexOf(key) >= 0 && otherTypes.includes(PromoType)) {
                                results.push(record);
                            }
                        }
                    }
                }
            }
        }
        return results;
    }
});