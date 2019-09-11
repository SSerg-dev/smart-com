Ext.define('App.extfilter.core.Range', {

    constructor: function (from, to) {
        this.from = from;
        this.to = to;
    },

    toString: function () {
        return Ext.String.format('{0} - {1}', this.from, this.to);
    }

});