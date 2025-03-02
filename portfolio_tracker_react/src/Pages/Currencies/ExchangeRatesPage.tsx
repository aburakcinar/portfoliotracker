import React, { useEffect, useRef, useState } from "react";
import { PageHeader } from "../../Controls/PageHeader";
import { Card } from "primereact/card";
import { CurrencyPicker } from "../../Controls/CurrencyPicker";
import { Calendar } from "primereact/calendar";
import * as d3 from "d3";
import {
  queryCurrencyExchangeRates,
  ICurrencyRateItem,
} from "../../Api/CurrencyExchangeRates.api";
import { ProgressSpinner } from "primereact/progressspinner";
import { Message } from "primereact/message";
import { InputText } from "primereact/inputtext";

export const ExchangeRatesPage: React.FC = () => {
  const svgRef = useRef<SVGSVGElement>(null);
  const [baseCurrency, setBaseCurrency] = useState<string>("EUR");
  const [targetCurrency, setTargetCurrency] = useState<string>("TRY");
  const [startDate, setStartDate] = useState<Date>(
    new Date(Date.now() - 30 * 24 * 60 * 60 * 1000)
  ); // 30 days ago
  const [endDate, setEndDate] = useState<Date>(new Date());
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<ICurrencyRateItem[]>([]);

  useEffect(() => {
    if (baseCurrency && targetCurrency && startDate && endDate) {
      fetchData();
    }
  }, [baseCurrency, targetCurrency, startDate, endDate]);

  const fetchData = async () => {
    try {
      setLoading(true);
      setError(null);
      const result = await queryCurrencyExchangeRates({
        startDate,
        endDate,
        baseCurrency,
        targetCurrency,
      });
      setData(result.rates);
      drawChart(result.rates);
    } catch (err) {
      setError("Failed to fetch exchange rate data. Please try again.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const drawChart = (chartData: ICurrencyRateItem[]) => {
    if (!svgRef.current || chartData.length === 0) return;

    // Clear previous chart
    d3.select(svgRef.current).selectAll("*").remove();

    const margin = { top: 20, right: 30, bottom: 30, left: 60 };
    const width = svgRef.current.clientWidth - margin.left - margin.right;
    const height = svgRef.current.clientHeight - margin.top - margin.bottom;

    const svg = d3
      .select(svgRef.current)
      .append("g")
      .attr("transform", `translate(${margin.left},${margin.top})`);

    // Create scales
    const xScale = d3
      .scaleTime()
      .domain(
        d3.extent(chartData, (d: ICurrencyRateItem) => new Date(d.date)) as [
          Date,
          Date
        ]
      )
      .range([0, width]);

    const yScale = d3
      .scaleLinear()
      .domain([
        (d3.min(chartData, (d: ICurrencyRateItem) => d.rate) ?? 0) * 0.95,
        (d3.max(chartData, (d: ICurrencyRateItem) => d.rate) ?? 0) * 1.05,
      ] as [number, number])
      .range([height, 0]);

    // Create line generator
    const line = d3
      .line<ICurrencyRateItem>()
      .x((d: ICurrencyRateItem) => xScale(new Date(d.date)))
      .y((d: ICurrencyRateItem) => yScale(d.rate));

    // Add axes
    svg
      .append("g")
      .attr("transform", `translate(0,${height})`)
      .call(d3.axisBottom(xScale));

    svg.append("g").call(d3.axisLeft(yScale));

    // Add line path
    svg
      .append("path")
      .datum(chartData)
      .attr("fill", "none")
      .attr("stroke", "steelblue")
      .attr("stroke-width", 1.5)
      .attr("d", line);

    // Add tooltip
    const tooltip = d3
      .select("body")
      .append("div")
      .attr("class", "tooltip")
      .style("position", "absolute")
      .style("background-color", "white")
      .style("padding", "5px")
      .style("border", "1px solid #ccc")
      .style("border-radius", "5px")
      .style("pointer-events", "none")
      .style("opacity", 0);

    // Add hover effects
    const focus = svg.append("g").style("display", "none");

    focus.append("circle").attr("r", 5).attr("fill", "steelblue");

    svg
      .append("rect")
      .attr("width", width)
      .attr("height", height)
      .style("fill", "none")
      .style("pointer-events", "all")
      .on("mouseover", () => {
        focus.style("display", null);
        tooltip.style("opacity", 1);
      })
      .on("mouseout", () => {
        focus.style("display", "none");
        tooltip.style("opacity", 0);
      })
      .on("mousemove", (event: MouseEvent) => {
        const bisect = d3.bisector(
          (d: ICurrencyRateItem) => new Date(d.date)
        ).left;
        const x0 = xScale.invert(d3.pointer(event)[0]);
        const i = bisect(chartData, x0, 1);

        // Ensure we have valid data points
        if (i > 0 && i < chartData.length && chartData[i - 1] && chartData[i]) {
          const d0 = chartData[i - 1];
          const d1 = chartData[i];
          const d =
            x0.getTime() - new Date(d0.date).getTime() >
            new Date(d1.date).getTime() - x0.getTime()
              ? d1
              : d0;

          focus.attr(
            "transform",
            `translate(${xScale(new Date(d.date))},${yScale(d.rate)})`
          );
          tooltip
            .html(
              `Date: ${new Date(
                d.date
              ).toLocaleDateString()}<br/>Rate: ${d.rate.toFixed(4)}`
            )
            .style("left", event.pageX + 10 + "px")
            .style("top", event.pageY - 10 + "px");
        }
      });
  };

  return (
    <div className="flex flex-col mt-20 w-full justify-center">
      <div className="flex flex-col w-3/4 mx-auto">
        <PageHeader title="Exchange Rates" />

        <Card className="mb-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="flex flex-col">
              <label className="mb-2 text-sm font-medium">Base Currency</label>
              <CurrencyPicker
                value={baseCurrency}
                onChange={(e) => setBaseCurrency(e ?? "")}
              />
            </div>
            <div className="flex flex-col">
              <label className="mb-2 text-sm font-medium">
                Target Currency
              </label>
              <CurrencyPicker
                value={targetCurrency}
                onChange={(e) => setTargetCurrency(e ?? "")}
              />
            </div>
            <div className="flex flex-col">
              <label className="mb-2 text-sm font-medium">Start Date</label>
              <Calendar
                value={startDate}
                onChange={(e) => setStartDate(e.value as Date)}
                showIcon
                dateFormat="dd/mm/yy"
                maxDate={endDate}
              />
            </div>
            <div className="flex flex-col">
              <label className="mb-2 text-sm font-medium">End Date</label>
              <Calendar
                value={endDate}
                onChange={(e) => setEndDate(e.value as Date)}
                showIcon
                dateFormat="dd/mm/yy"
                minDate={startDate}
                maxDate={new Date()}
              />
            </div>
          </div>
        </Card>

        <Card title="Exchange Rate Chart" className="relative">
          {loading && (
            <div className="absolute inset-0 flex items-center justify-center bg-white/50">
              <ProgressSpinner />
            </div>
          )}
          {error && <Message severity="error" text={error} className="mb-4" />}
          {!loading && !error && data.length === 0 && (
            <Message
              severity="info"
              text="No exchange rate data available. Please select currencies and date range."
            />
          )}
          <svg ref={svgRef} className="w-full h-96" />
        </Card>
      </div>
    </div>
  );
};
