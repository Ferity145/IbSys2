﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Ibsys2.Static;
using Ibsys2.Static.Input;
using System.IO;

namespace Ibsys2.Pages.ReadXML {
    /// <summary>
    /// Interaktionslogik für ReadXML.xaml
    /// </summary>
    public partial class ReadXML : Page {
        public ReadXML() {
            InitializeComponent();
        }


        public void KlickAufDateiUpload() {
            //if File Exists

        }

        public void KlickAufTextUpload() {
            //If not Null


        }

        public void ReadFile(string filepath)
        {
            if (String.IsNullOrEmpty(filepath))
            {
                MessageBox.Show("Missing Filepath", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string xmlinput = File.ReadAllText(filepath);
            ParseXML(xmlinput);
        }

        public void ParseXML(string XMLinput) {
            if (String.IsNullOrEmpty(XMLinput))
                throw new ArgumentNullException();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XMLinput);
            Static.Static.game = Convert.ToInt32(doc.DocumentElement.Attributes["game"].InnerText);
            Static.Static.group = Convert.ToInt32(doc.DocumentElement.Attributes["group"].InnerText);
            Static.Static.period = Convert.ToInt32(doc.DocumentElement.Attributes["period"].InnerText);

            foreach (XmlNode Results in doc.DocumentElement.ChildNodes) {
                foreach (XmlNode node in Results) {
                    if (Results.Name == "warehousestock") {
                        Warehousestock w = Warehousestock.Class;
                        if (node.Name == "article")
                            w.AddArticle(new Warehousestock.Article(Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["amount"].InnerText), Convert.ToInt32(node.Attributes["startamount"].InnerText), Convert.ToDouble(node.Attributes["pct"].InnerText), Convert.ToDouble(node.Attributes["price"].InnerText), Convert.ToDouble(node.Attributes["stockvalue"].InnerText)));
                    } else if (Results.Name == "inwardstockmovement") {
                        Inwardstockmovement iwsm = Inwardstockmovement.Class;
                        iwsm.AddOrder(new Inwardstockmovement.Order(Convert.ToInt32(node.Attributes["orderperiod"].InnerText), Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["mode"].InnerText), Convert.ToInt32(node.Attributes["article"].InnerText), Convert.ToInt32(node.Attributes["amount"].InnerText), Convert.ToInt32(node.Attributes["time"].InnerText), Convert.ToDouble(node.Attributes["materialcosts"].InnerText), Convert.ToDouble(node.Attributes["ordercosts"].InnerText), Convert.ToDouble(node.Attributes["entirecosts"].InnerText), Convert.ToDouble(node.Attributes["piececosts"].InnerText)));
                    } else if (Results.Name == "futureinwardstockmovement") {
                        Futureinwardstockmovement fiwsm = Futureinwardstockmovement.Class;
                        fiwsm.AddOrder(new Futureinwardstockmovement.Order(Convert.ToInt32(node.Attributes["orderperiod"].InnerText), Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["mode"].InnerText), Convert.ToInt32(node.Attributes["article"].InnerText), Convert.ToInt32(node.Attributes["amount"].InnerText)));
                    } else if (Results.Name == "idletimecosts") {
                        Idletimecosts itc = Idletimecosts.Class;
                        if (node.Name == "workplace")
                            itc.AddWorkplace(new Idletimecosts.Workplace(Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["setupevents"].InnerText), Convert.ToInt32(node.Attributes["idletime"].InnerText), Convert.ToDouble(node.Attributes["wageidletimecosts"].InnerText), Convert.ToDouble(node.Attributes["wagecosts"].InnerText), Convert.ToDouble(node.Attributes["machineidletimecosts"].InnerText)));
                    } else if (Results.Name == "waitinglistworkstations") {
                        Waitinglistworkstations wlws = Waitinglistworkstations.Class;
                        Waitinglistworkstations.Workplace wp = new Waitinglistworkstations.Workplace(Convert.ToInt32(node.Attributes["id"].InnerText));
                        foreach (XmlNode waitinglist in node)
                            wp.Add(new Waitinglistworkstations.Workplace.Waitinglist(Convert.ToInt32(waitinglist.Attributes["period"].InnerText), Convert.ToInt32(waitinglist.Attributes["order"].InnerText), Convert.ToInt32(waitinglist.Attributes["firstbatch"].InnerText), Convert.ToInt32(waitinglist.Attributes["lastbatch"].InnerText), Convert.ToInt32(waitinglist.Attributes["item"].InnerText), Convert.ToInt32(waitinglist.Attributes["amount"].InnerText), Convert.ToInt32(waitinglist.Attributes["timeneed"].InnerText)));
                        wlws.Add(wp);
                    } else if (Results.Name == "waitingliststock") {
                        Waitingliststock wls = Waitingliststock.Class;
                        Waitingliststock.Missingpart mp = new Waitingliststock.Missingpart(Convert.ToInt32(node.Attributes["id"].InnerText));
                        foreach (XmlNode waitinglist in node)
                            mp.Add(new Waitingliststock.Missingpart.Waitinglist(Convert.ToInt32(waitinglist.Attributes["period"].InnerText), Convert.ToInt32(waitinglist.Attributes["order"].InnerText), Convert.ToInt32(waitinglist.Attributes["firstbatch"].InnerText), Convert.ToInt32(waitinglist.Attributes["lastbatch"].InnerText), Convert.ToInt32(waitinglist.Attributes["item"].InnerText), Convert.ToInt32(waitinglist.Attributes["amount"].InnerText)));
                        wls.Add(mp);

                    } else if (Results.Name == "ordersinwork") {
                        Ordersinwork oiw = Ordersinwork.Class;
                        oiw.Add(new Ordersinwork.Workplace(Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["period"].InnerText), Convert.ToInt32(node.Attributes["order"].InnerText), Convert.ToInt32(node.Attributes["batch"].InnerText), Convert.ToInt32(node.Attributes["item"].InnerText), Convert.ToInt32(node.Attributes["amount"].InnerText), Convert.ToInt32(node.Attributes["timeneed"].InnerText)));
                    } else if (Results.Name == "completedorders") {
                        Completedorders co = Completedorders.Class;
                        Completedorders.Order o = new Completedorders.Order(Convert.ToInt32(node.Attributes["period"].InnerText), Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["item"].InnerText));
                        foreach (XmlNode batch in node)
                            o.AddBatch(new Completedorders.Order.Batch(Convert.ToInt32(batch.Attributes["id"].InnerText), Convert.ToInt32(batch.Attributes["amount"].InnerText), Convert.ToInt32(batch.Attributes["cycletime"].InnerText), Convert.ToDouble(batch.Attributes["cost"].InnerText)));
                        co.AddOrder(o);

                    } else if (Results.Name == "cycletimes") {
                        Cycletimes ct = Cycletimes.Class;
                        if (Cycletimes.Class == null)
                            ct = new Cycletimes(Convert.ToInt32(Results.Attributes["startedorders"].InnerText), Convert.ToInt32(Results.Attributes["waitingorders"].InnerText));
                        ct.AddOrder(new Cycletimes.Order(Convert.ToInt32(node.Attributes["id"].InnerText), Convert.ToInt32(node.Attributes["period"].InnerText), node.Attributes["starttime"].InnerText, node.Attributes["finishtime"].InnerText, Convert.ToInt32(node.Attributes["cycletimemin"].InnerText), Convert.ToDouble(node.Attributes["cycletimefactor"].InnerText)));
                    } else if (Results.Name == "result") {
                        foreach (XmlNode inner in node) {
                            if (node.Name == "general") {
                                if (inner.Name == "capacity") Result.General.Capacity = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "possiblecapacity") Result.General.Possiblecapacity = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "relpossiblenormalcapacity") Result.General.Relpossiblenormalcapacity = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "productivetime") Result.General.Productivetime = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "effiency") Result.General.Effiency = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "sellwish") Result.General.Sellwish = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "salesquantity") Result.General.Salesquantity = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "deliveryreliability") Result.General.Deliveryreliability = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "idletime") Result.General.Idletime = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "idletimecosts") Result.General.Idletimecosts = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "storevalue") Result.General.Storevalue = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "storagecosts") Result.General.Storagecosts = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            } else if (node.Name == "defectivegoods") {
                                if (inner.Name == "quantity") Result.Defectivegoods.Quantity = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "costs") Result.Defectivegoods.Costs = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            } else if (node.Name == "normalsale") {
                                if (inner.Name == "salesprice") Result.Normalsale.Salesprice = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "profit") Result.Normalsale.Profit = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "profitperunit") Result.Normalsale.Profitperunit = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            } else if (node.Name == "directsale") {
                                if (inner.Name == "profit") Result.Directsale.Profit = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                                else if (inner.Name == "contractpenalty") Result.Directsale.Contractpenalty = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            } else if (node.Name == "marketplacesale") {
                                if (inner.Name == "profit") Result.Marketplacesale.Profit = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            } else if (node.Name == "summary") {
                                if (inner.Name == "profit") Result.Summary.Profit = new Result.ResultTyp(inner.Attributes["current"].InnerText, inner.Attributes["average"].InnerText, inner.Attributes["all"].InnerText);
                            }
                        }
                    }
                }
            }
        }
    }
}
