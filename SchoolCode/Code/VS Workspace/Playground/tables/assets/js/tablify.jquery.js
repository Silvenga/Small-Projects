
$(function () {

    var tableSelector = "#table";

    var $table = $(tableSelector);

    window.s = new SlightTable($table);
});


var SlightTable = function ($table) {

    var $raw = $table.find("tr");
    var $header = $raw.slice(0, 1);
    var $data = $raw.slice(1, $raw.length);

    this.table = $table;
    this.header = $header;
    this.data = $data;
    this.filtered = $data;

    this.wrapper = this.table.wrap($("<div/>", { "class": "slight-table" })).parent();

    this.settings = {
        maxRows: 1,
        index: 1,
        pages: 1,
        hiddenColumns: []
    };

    this.setupFilter();
    this.createCheckboxes();
    this.setupPagination();
    this.showPage(1);
}

SlightTable.prototype.next = function () {

    if (this.settings.index < this.settings.pages) {
        this.showPage(++this.settings.index);
    }
}

SlightTable.prototype.prev = function () {

    if (this.settings.index > 1) {
        this.showPage(--this.settings.index);
    }
}

SlightTable.prototype.showPage = function (index) {

    this.settings.index = index;

    this.drawTable();
}

SlightTable.prototype.setupFilter = function () {

    this.filter = $("<input/>", {
        "type": "text",
        "class": "form-control",
        "width": "200px"
    });

    var parrent = this;
    this.filter.on("keyup", function () {
        parrent.drawTable();
    });

    this.wrapper.prepend(this.filter);
}

SlightTable.prototype.setupPagination = function () {

    this.pagerContainer = $("<nav/>");

    this.wrapper.append(this.pagerContainer);

    this.drawPager();
}

SlightTable.prototype.drawPager = function () {

    var parrent = this;
    var rowsPerPage = this.settings.maxRows;
    var count = this.filtered.length;

    var pages = Math.floor(count / rowsPerPage);
    if (count % rowsPerPage > 0) {
        pages++;
    }

    this.settings.pages = pages;

    var pager = $('<ul class="pagination"></ul>');

    pager.prepend(
        $('<li id="prev"><a href="#"><span>&laquo;</span></a></li>').on("click", function () {
            parrent.prev();
        })
    );
    pager.append(
        $('<li id="next"><a href="#"><span>&raquo;</span></a></li>').on("click", function () {
            parrent.next();
        })
    );

    for (var i = 1; i <= pages; i++) {

        var active = (this.settings.index == i) ? "active" : "";
        var $nextIndex = $('<li><a href="#">' + i + '</a></li>').addClass(active)
            .on("click", function () {
                var pageNum = $(this).find("a").text();
                parrent.showPage(pageNum);
            });
        pager.find("#next").before($nextIndex);
    }

    this.pagerContainer.html(pager);
}

SlightTable.prototype.createCheckboxes = function () {

    var checkboxes = $("<div/>");
    var parrent = this;

    this.table.find('th').each(function (col) {

        var index = col + 1;

        var $div = $('<div />', {
            "class": "checkbox"
        });

        var $label = $('<label />').text($(this).text());

        var $checkbox = $('<input />', {
            type: 'checkbox',
            id: index
        }).prop('checked', true);

        $checkbox.change(function () {

            if ($(this).is(":checked")) {
                parrent.showColumn(index);
            } else {
                parrent.hideColumn(index);
            }
        });

        $checkbox.prependTo($label.appendTo($div.appendTo(checkboxes)));
    });

    this.wrapper.prepend(checkboxes);
}

SlightTable.prototype.drawTable = function () {

    var page = this.settings.index;

    var $rowsPerPage = this.settings.maxRows;
    var $rowStart = $rowsPerPage * (page - 1);
    var $rowEnd = $rowsPerPage * (page);

    // Filter
    var searchText = this.filter.val();
    this.filtered = this.data;

    if (searchText.length > 0) {

        searchText = searchText.toLocaleLowerCase();

        this.filtered = this.filtered.filter(function (index, item) {

            var contains = false;
            $(item).find("td").each(function () {
                contains = contains || $(this).text().toLocaleLowerCase().indexOf(searchText) > -1;
            });

            return contains;
        });
    }

    // Sort
    this.sortData();

    // Draw
    this.table.html(this.header);
    this.table.append(this.filtered.slice($rowStart, $rowEnd));

    // Fix Chrome's default tbody issues
    this.table.each(function () {
        var $this = $(this);
        $this.children('tbody').children().unwrap();
        $this.children('tr:has(th)').wrapAll('<thead>');
        $this.children('tr:has(td)').wrapAll('<tbody>');
    });

    var parrent = this;
    this.table.find("th").each(function (index) {
        $(this).click(function () {
            index++;
            parrent.sortColumn(index);
            parrent.drawTable();
        });
    });

    this.table.find("td,th").each(function () {
        $(this).show();
    });

    for (var i = 0; i < this.settings.hiddenColumns.length; i++) {
        var column = this.settings.hiddenColumns[i];
        this.table.find("td:nth-child(" + column + "),th:nth-child(" + column + ")").hide();
    }

    this.drawPager();
}

SlightTable.prototype.hideColumn = function (index) {

    this.settings.hiddenColumns.push(index);
    this.drawTable();
}

SlightTable.prototype.showColumn = function (index) {

    var i = this.settings.hiddenColumns.indexOf(index);

    if (i > -1) {
        this.settings.hiddenColumns.splice(i, 1);
    }

    this.drawTable();
}

SlightTable.prototype.sortData = function () {

    var parrent = this;

    var index = this.header.find("th").index($("[data-sort]"));
    var sort = $(this.header.find("th")[index]).data("sort");
    var reverse = $(this.header.find("th")[index]).data("reverse");

    if (index >= 0) {
        this.filtered = this.filtered.sort(function (a, b) {

            var aText = $($(a).find("td")[index]).text();
            var bText = $($(b).find("td")[index]).text();

            return parrent.comparer(aText, bText, sort);
        });

        if (reverse) {
            this.filtered = $(this.filtered.get().reverse());
        }
    }
}

SlightTable.prototype.sortColumn = function (index) {

    index--;
    var sort = this.findSortType(index);

    this.header.find("th").each(function (i) {

        if (index == i) {

            $(this).attr("data-sort", sort);

            var reverse = ($(this).attr("data-reverse") == undefined) ? false : !$(this).data("reverse");
            $(this).attr("data-reverse", reverse);

        } else {
            $(this).removeAttr("data-reverse");
            $(this).removeAttr("data-sort");
        }
    });

    this.drawTable();
}

SlightTable.prototype.comparer = function (a, b, sort) {

    if (sort == this.sortType.number) {
        return parseFloat(a) - parseFloat(b);
    } else if (sort == this.sortType.word) {
        return a.toLocaleLowerCase().localeCompare(b.toLocaleLowerCase());
    } else {
        return a.toLocaleLowerCase().localeCompare(b.toLocaleLowerCase());
    }
}

// Find the sort type for a column
SlightTable.prototype.findSortType = function (index) {

    var isNum = true;
    for (var i = 0; i < this.filtered.length && isNum; i++) {

        var value = $($(this.filtered[i]).find("td")[index]).text();

        if (isNaN(parseFloat(value))) {
            isNum = false;
        }
    }

    return (isNum) ? this.sortType.number : this.sortType.word;
}

SlightTable.prototype.sortType = {
    number: 0,
    word: 1
}