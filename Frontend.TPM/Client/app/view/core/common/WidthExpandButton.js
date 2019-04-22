Ext.define('App.view.core.common.WidthExpandButton', {
    extend: 'App.view.core.common.ExpandButton',
    alias: 'widget.widthexpandbutton',

    toggleCollapse: function () {
        var target = this.getTarget();

        target.setWidth(this.isCollapsed() ? target.maxWidth : target.minWidth);
        target.isExpanded = !target.isExpanded;
    },

    isCollapsed: function () {
        return !this.getTarget().isExpanded;
    }

});