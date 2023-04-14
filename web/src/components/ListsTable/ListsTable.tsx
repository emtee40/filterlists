import { OData, FilterList } from "@/src/interfaces";
import { Table, TablePaginationConfig } from "antd";
import { coalesceToArray, localeCompare } from "@/src/utils";
import { useState } from "react";
import {
  FilterValue,
  SorterResult,
  TableCurrentDataSource,
} from "antd/es/table/interface";

export const ListsTable = (props: OData<FilterList>) => {
  const [data, setData] = useState(props.value);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: props["@odata.count"] ?? 0,
  });
  const [loading, setLoading] = useState(false);

  const handleTableChange = async (
    pagination: TablePaginationConfig,
    _filters: Record<string, FilterValue | null>,
    sorter: SorterResult<FilterList> | SorterResult<FilterList>[],
    _extra: TableCurrentDataSource<FilterList>
  ) => {
    setLoading(true);
    const params = new URLSearchParams({
      $count: "true",
      $skip: (
        ((pagination.current ?? 0) - 1) *
        (pagination.pageSize ?? 0)
      ).toString(),
      $top: (pagination.pageSize ?? 0).toString(),
    });
    const orderBy = toODataOrderBy(sorter);
    if (orderBy.length) {
      params.append("$orderby", orderBy);
    }
    const url =
      `${process.env.NEXT_PUBLIC_FILTERLISTS_API_URL}/lists?` + params;
    const response = await fetch(url);
    const jsonData = await response.json();
    setData(jsonData.value);
    setPagination({
      current: pagination.current ?? 0,
      pageSize: pagination.pageSize ?? 0,
      total: jsonData["@odata.count"] ?? 0,
    });
    setLoading(false);
  };

  return (
    <Table<FilterList>
      dataSource={data}
      rowKey={(record) => record.id}
      pagination={pagination}
      loading={loading}
      onChange={handleTableChange}
    >
      {nameColumn}
      {descriptionColumn}
    </Table>
  );
};

// TODO: multiple sorter https://ant.design/components/table#components-table-demo-multiple-sorter
const toODataOrderBy = (
  sorter: SorterResult<FilterList> | SorterResult<FilterList>[]
) => {
  const sorterArray = coalesceToArray(sorter);
  let orderBy = "";
  sorterArray.forEach((element) => {
    if (element.order) {
      orderBy += `${element.field} ${
        element.order === "descend" ? "desc" : "asc"
      }`;
    }
  });
  return orderBy;
};

const nameColumn = (
  <Table.Column<FilterList>
    dataIndex="name"
    title="Name"
    sorter={(a, b) => localeCompare({ a: a.name, b: b.name })}
  />
);

const descriptionColumn = (
  <Table.Column<FilterList>
    dataIndex="description"
    title="Description"
    sorter={(a, b) => localeCompare({ a: a.description, b: b.description })}
  />
);
