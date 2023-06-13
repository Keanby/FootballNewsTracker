import { Apollo, APOLLO_OPTIONS, ApolloModule } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ApolloClientOptions, ApolloLink, InMemoryCache, split } from '@apollo/client/core';
import { setContext } from '@apollo/client/link/context';
import { WebSocketLink } from '@apollo/client/link/ws';
import { getMainDefinition } from '@apollo/client/utilities';


const uri = 'https://localhost:53969/graphql/'; // <-- add the URL of the GraphQL server here
const ws_uri = 'ws://localhost:55036/graphql/';
export function createApollo(httpLink: HttpLink): ApolloClientOptions<any> {

  // Create a WebSocket link:
  const ws = new WebSocketLink({
    uri: ws_uri,
    options: {
      reconnect: true,
    },
  });


  const basic = setContext((operation, context) => ({
    headers: {
      'Content-Type': 'application/json',
    },
  }));
  const auth = setContext((operation, context) => {
    const token = localStorage.getItem('token');

    if (token === null) {
      return {};
    } else {
      return {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      };
    }
  });


  const link = split(
    // split based on operation type
    ({ query }) => {
      let definition = getMainDefinition(query);
      return definition.kind === 'OperationDefinition' && definition.operation === 'subscription';
    },
    ws,
    ApolloLink.from([basic, auth, httpLink.create({ uri: uri })]),
  );

  return {
    link: link,
    /*    link: ApolloLink.from([basic, auth, httpLink.create({ uri: uri })]),*/
    /*    link: httpLink.create({ uri }),*/
    cache: new InMemoryCache(),
  };
}

@NgModule({
  exports: [ApolloModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
})
export class GraphQLModule { }
