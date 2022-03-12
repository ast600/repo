import pandas as pd
import dash
import dash_html_components as html
import dash_core_components as dcc
from dash.dependencies import Input, Output
import plotly.express as px

spacex_df = pd.read_csv("spacex_launch_dash.csv")
max_payload = spacex_df['Payload Mass (kg)'].max()
min_payload = spacex_df['Payload Mass (kg)'].min()

app = dash.Dash(__name__)

app.layout = html.Div(children=[html.H1('SpaceX Launch Records Dashboard',
                                        style={'textAlign': 'center', 'color': '#503D36',
                                               'font-size': 40}),
                                dcc.Dropdown(id='site-dropdown',
                                        options=[{'label': 'All Sites', 'value': 'ALL'},
                                                {'label': 'CCAFS LC-40', 'value': 'CCAFS LC-40'},
                                                {'label': 'VAFB SLC-4E', 'value': 'VAFB SLC-4E'},
                                                {'label': 'KSC LC-39A', 'value': 'KSC LC-39A'},
                                                {'label': 'CCAFS SLC-40', 'value': 'CCAFS SLC-40'}],
                                        value='ALL',
                                        placeholder="Select a Launch Site here",
                                        searchable=True),
                                html.Br(),

                                html.Div(dcc.Graph(id='success-pie-chart')),
                                html.Br(),

                                html.P("Payload range (Kg):"),
                                dcc.RangeSlider(id='payload-slider',
                                                min=0, max=10000, step=1000,
                                                marks={0: '0 kg',
                                                        2500: '2500 kg',
                                                        5000: '5000 kg',
                                                        7500: '7500 kg',
                                                        10000: '10000 kg'},
                                                value=[2500, 7500]),

                                html.Div(dcc.Graph(id='success-payload-scatter-chart')),
                                ])

@app.callback(Output(component_id='success-pie-chart', component_property='figure'),
            Input(component_id='site-dropdown', component_property='value'))
def get_pie_chart(entered_site):
            if entered_site == 'ALL':
                fig = px.pie(spacex_df, values='class',
                                        names='Launch Site',
                                        title='Total Success Launches by Site')
                return fig
            else:
                f_df=spacex_df[spacex_df['Launch Site']== entered_site]
                f_df=f_df.groupby(['Launch Site','class']).size().reset_index(name='class count')
                fig1=px.pie(f_df,values='class count',names='class',title=f"Total Success Launches for site {entered_site}")
                return fig1

@app.callback(Output(component_id='success-payload-scatter-chart',component_property='figure'),
                [Input(component_id='site-dropdown',component_property='value'),
                Input(component_id='payload-slider',component_property='value')])
def scatter(entered_site,payload):
    payload_df = spacex_df[spacex_df['Payload Mass (kg)'].between(payload[0],payload[1])]    
    if entered_site=='ALL':
        fig2=px.scatter(payload_df,x='Payload Mass (kg)',y='class',color='Booster Version Category',title='Correlation between Payload Mass and Success for all sites')
        fig2.update_xaxes(range=[0, 10000])
        return fig2
    else:
        fig3=px.scatter(payload_df.loc[payload_df['Launch Site']==entered_site],x='Payload Mass (kg)',y='class',color='Booster Version Category',title=f"Correlation between Payload Mass and Success for all sites for site {entered_site}")
        fig3.update_xaxes(range=[0, 10000])
        return fig3

if __name__ == '__main__':
    app.run_server()