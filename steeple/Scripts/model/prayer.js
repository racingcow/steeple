(function ($, ko, window) {

    window.Steeple = window.Steeple || {};

    window.Steeple.Prayer = function(title, entries, status, tags) {
        this.title = ko.observable(title);
        this.entries = ko.observableArray(entries);
        this.status = 1;
        this.tags = ko.observableArray(tags);
    }

} (jQuery, ko, window));